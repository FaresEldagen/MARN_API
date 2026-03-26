namespace MARN_API.Models
{
    public class Owner : ApplicationUser
    {
        public virtual ICollection<Property> Properties { get; set; } = new HashSet<Property>();
    }
}
