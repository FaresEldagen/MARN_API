using MARN_API.Enums.Contract;
using MARN_API.Repositories.Interfaces;
using MARN_API.Services.Interfaces;

namespace MARN_API.BackgroundJobs
{
    public class OtsUpgradeJob
    {
        private readonly IContractRepo _contractRepo;
        private readonly IOpenTimestampsService _openTimestampsService;
        private readonly IOpenTimestampsProofReader _openTimestampsProofReader;
        private readonly ILogger<OtsUpgradeJob> _logger;

        public OtsUpgradeJob(
            IContractRepo contractRepo,
            IOpenTimestampsService openTimestampsService,
            IOpenTimestampsProofReader openTimestampsProofReader,
            ILogger<OtsUpgradeJob> logger)
        {
            _contractRepo = contractRepo;
            _openTimestampsService = openTimestampsService;
            _openTimestampsProofReader = openTimestampsProofReader;
            _logger = logger;
        }


        public async Task ExecuteAsync()
        {
            try
            {
                var pendingContracts = await _contractRepo.GetPendingContractsAsync();

                foreach (var contract in pendingContracts)
                {
                    try
                    {
                        if (contract.OtsFileBytes is null || contract.OtsFileBytes.Length == 0)
                        {
                            continue;
                        }

                        var updatedOts = await _openTimestampsService.UpgradeOtsAsync(contract.OtsFileBytes);
                        if (updatedOts is null)
                        {
                            continue;
                        }

                        var proofData = _openTimestampsProofReader.Extract(updatedOts);
                        contract.OtsFileBytes = updatedOts;
                        contract.AnchoringStatus = ContractAnchoringStatus.Anchored;
                        contract.AnchoredAt = DateTime.UtcNow;
                        contract.TransactionId = proofData.TransactionIds.FirstOrDefault();
                        contract.MerkleRoot = proofData.MerkleRoots.FirstOrDefault();
                        await _contractRepo.UpdateAsync(contract);
                    }
                    catch (Exception innerEx)
                    {
                        _logger.LogError(innerEx, "Error upgrading OTS proof for contract {ContractId}", contract.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in OtsUpgradeJob.");
                throw;
            }
        }
    }
}
