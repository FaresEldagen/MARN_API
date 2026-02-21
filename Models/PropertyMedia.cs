namespace MARN_API.Models
{
    public class PropertyMedia
    {
        public long Id { get; set; }
        public long PropertyId { get; set; }
        public string Path { get; set; } = string.Empty;

        public virtual Property Property { get; set; } = null!;
    }
}



