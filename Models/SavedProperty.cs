namespace MARN_API.Models
{
    public class SavedProperty
    {
        public long PropertyId { get; set; }
        public Guid UserId { get; set; }

        public virtual Property Property { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;
    }
}
