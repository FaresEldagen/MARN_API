using MARN_API.Models;

namespace MARN_API.Services.Interfaces
{
    public interface IConnectedAccountService
    {
        Task<ConnectedAccount> CreateConnectedAccountAsync(Guid applicationUserId);
        Task<string> GenerateOnboardingLinkAsync(Guid connectedAccountId, string returnUrl, string refreshUrl);
        Task<bool> VerifyOnboardingCompletionAsync(Guid connectedAccountId);
        Task<IEnumerable<ConnectedAccount>> GetAllConnectedAccountsAsync();
        Task<ConnectedAccount?> GetConnectedAccountByIdAsync(Guid connectedAccountId);
        Task<ConnectedAccount?> GetConnectedAccountByApplicationUserIdAsync(Guid applicationUserId);
    }
}
