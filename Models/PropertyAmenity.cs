using MARN_API.Enums;

namespace MARN_API.Models
{
    public class PropertyAmenity
    {
        public long PropertyId { get; set; }
        public AmenityType Amenity { get; set; }

        public virtual Property Property { get; set; } = null!;
    }
}



