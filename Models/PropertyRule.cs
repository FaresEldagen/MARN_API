namespace MARN_API.Models
{
    public class PropertyRule
    {
        public long Id { get; set; }
        public long PropertyId { get; set; }
        public string Rule { get; set; } = string.Empty;

        public virtual Property Property { get; set; } = null!;
    }
}



