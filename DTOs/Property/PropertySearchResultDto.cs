namespace MARN_API.DTOs.Property
{
    /// <summary>
    /// Paginated wrapper for property search results.
    /// </summary>
    public class PropertySearchResultDto
    {
        public List<PropertyCardDto> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
