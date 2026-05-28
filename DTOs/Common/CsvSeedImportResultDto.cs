namespace MARN_API.DTOs.Common
{
    public class CsvSeedImportResultDto
    {
        public string SeedType { get; set; } = string.Empty;
        public int TotalRows { get; set; }
        public int ImportedRows { get; set; }
        public int SkippedRows { get; set; }
        public List<string> Messages { get; set; } = new();
    }
}
