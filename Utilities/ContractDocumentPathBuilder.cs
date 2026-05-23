namespace MARN_API.Utilities
{
    public static class ContractDocumentPathBuilder
    {
        private const string StorageRootFolder = "Storage";
        private const string ContractsFolder = "contracts";

        public static string BuildPdfRelativePath(long contractId)
        {
            return BuildRelativePath(contractId, $"{contractId}.pdf");
        }

        public static string BuildOtsRelativePath(long contractId)
        {
            return BuildRelativePath(contractId, $"{contractId}.ots");
        }

        private static string BuildRelativePath(long contractId, string fileName)
        {
            return Path.Combine(StorageRootFolder, ContractsFolder, contractId.ToString(), fileName)
                .Replace("\\", "/");
        }
    }
}
