using MARN_API.Enums;
using MARN_API.Repositories.Interfaces;

namespace MARN_API.Services.Implementations
{
    public class OtsUpgradeBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<OtsUpgradeBackgroundService> _logger;

        public OtsUpgradeBackgroundService(IServiceProvider serviceProvider, ILogger<OtsUpgradeBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var interval = TimeSpan.FromHours(1);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var repo = scope.ServiceProvider.GetRequiredService<IWorkflowContractRepo>();
                    var otsService = scope.ServiceProvider.GetRequiredService<OpenTimestampsService>();
                    var proofReader = scope.ServiceProvider.GetRequiredService<OpenTimestampsProofReader>();
                    var rentalRepo = scope.ServiceProvider.GetRequiredService<IRentalTransactionRepo>();

                    var pendingContracts = await repo.GetPendingContractsAsync();
                    foreach (var contract in pendingContracts)
                    {
                        try
                        {
                            if (contract.OtsFileBytes is null || contract.OtsFileBytes.Length == 0)
                            {
                                continue;
                            }

                            var updatedOts = await otsService.UpgradeOtsAsync(contract.OtsFileBytes);
                            if (updatedOts is null)
                            {
                                continue;
                            }

                            var proofData = proofReader.Extract(updatedOts);
                            contract.OtsFileBytes = updatedOts;
                            contract.AnchoringStatus = ContractAnchoringStatus.Anchored;
                            contract.AnchoredAt = DateTime.UtcNow;
                            contract.TransactionId = proofData.TransactionIds.FirstOrDefault();
                            contract.MerkleRoot = proofData.MerkleRoots.FirstOrDefault();
                            contract.UpdatedAt = DateTime.UtcNow;
                            await repo.UpdateAsync(contract);

                            var transaction = await rentalRepo.GetByContractIdAsync(contract.Id);
                            if (transaction is not null)
                            {
                                transaction.Status = RentalTransactionStatus.Anchored;
                                transaction.CompletedAt = DateTime.UtcNow;
                                await rentalRepo.UpdateAsync(transaction);
                            }
                        }
                        catch (Exception innerEx)
                        {
                            _logger.LogError(innerEx, "Error upgrading OTS proof for contract {ContractId}", contract.Id);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred in OtsUpgradeBackgroundService.");
                }

                await Task.Delay(interval, stoppingToken);
            }
        }
    }
}
