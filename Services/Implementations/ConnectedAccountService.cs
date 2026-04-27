using Microsoft.AspNetCore.Identity;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using MARN_API.Services.Interfaces;
using Stripe;

namespace MARN_API.Services.Implementations
{
    public class ConnectedAccountService : IConnectedAccountService
    {
        private readonly IConnectedAccountRepo _connectedAccountRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public ConnectedAccountService(IConnectedAccountRepo connectedAccountRepo, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _connectedAccountRepo = connectedAccountRepo;
            _userManager = userManager;
            _configuration = configuration;
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
        }

        public async Task<ConnectedAccount> CreateConnectedAccountAsync(Guid applicationUserId)
        {
            var existing = await _connectedAccountRepo.GetByApplicationUserIdAsync(applicationUserId);
            if (existing is not null)
            {
                return existing;
            }

            var user = await _userManager.FindByIdAsync(applicationUserId.ToString())
                ?? throw new InvalidOperationException("Owner user not found.");

            var accountOptions = new AccountCreateOptions
            {
                Type = "express",
                Email = user.Email,
                Capabilities = new AccountCapabilitiesOptions
                {
                    Transfers = new AccountCapabilitiesTransfersOptions
                    {
                        Requested = true
                    }
                },
                BusinessProfile = new AccountBusinessProfileOptions
                {
                    Name = $"{user.FirstName} {user.LastName}".Trim()
                }
            };

            var accountService = new Stripe.AccountService();
            var stripeAccount = await accountService.CreateAsync(accountOptions);

            var connectedAccount = new ConnectedAccount
            {
                ApplicationUserId = applicationUserId,
                StripeAccountId = stripeAccount.Id,
                IsOnboardingComplete = false
            };

            await _connectedAccountRepo.AddAsync(connectedAccount);
            return connectedAccount;
        }

        public async Task<string> GenerateOnboardingLinkAsync(Guid connectedAccountId, string returnUrl, string refreshUrl)
        {
            var connectedAccount = await _connectedAccountRepo.GetByIdAsync(connectedAccountId)
                ?? throw new InvalidOperationException("Connected account not found.");

            if (string.IsNullOrWhiteSpace(connectedAccount.StripeAccountId))
            {
                throw new InvalidOperationException("Connected account does not have a Stripe account attached.");
            }

            var linkOptions = new AccountLinkCreateOptions
            {
                Account = connectedAccount.StripeAccountId,
                RefreshUrl = refreshUrl,
                ReturnUrl = returnUrl,
                Type = "account_onboarding"
            };

            var linkService = new AccountLinkService();
            var link = await linkService.CreateAsync(linkOptions);
            return link.Url;
        }

        public async Task<bool> VerifyOnboardingCompletionAsync(Guid connectedAccountId)
        {
            var connectedAccount = await _connectedAccountRepo.GetByIdAsync(connectedAccountId);
            if (connectedAccount is null || string.IsNullOrWhiteSpace(connectedAccount.StripeAccountId))
            {
                return false;
            }

            var accountService = new Stripe.AccountService();
            var account = await accountService.GetAsync(connectedAccount.StripeAccountId);

            if (account.ChargesEnabled && account.PayoutsEnabled)
            {
                connectedAccount.IsOnboardingComplete = true;
                connectedAccount.UpdatedAt = DateTime.UtcNow;
                await _connectedAccountRepo.UpdateAsync(connectedAccount);
                return true;
            }

            return false;
        }

        public Task<IEnumerable<ConnectedAccount>> GetAllConnectedAccountsAsync()
        {
            return _connectedAccountRepo.GetAllAsync();
        }

        public Task<ConnectedAccount?> GetConnectedAccountByIdAsync(Guid connectedAccountId)
        {
            return _connectedAccountRepo.GetByIdAsync(connectedAccountId);
        }

        public Task<ConnectedAccount?> GetConnectedAccountByApplicationUserIdAsync(Guid applicationUserId)
        {
            return _connectedAccountRepo.GetByApplicationUserIdAsync(applicationUserId);
        }
    }
}
