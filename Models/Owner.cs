namespace MARN_API.Models
{
    public class Owner : ApplicationUser
    {
        public string? StripeAccountId { get; set; }
        public bool StripePayoutsEnabled { get; set; }
        public bool StripeChargesEnabled { get; set; }

        public virtual ICollection<Property> Properties { get; set; } = new HashSet<Property>();
    }
}
