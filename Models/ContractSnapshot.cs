using System;

namespace MARN_API.Models
{
    public class ContractSnapshot
    {
        public UserSnapshot RenterSnapshot { get; set; } = new();
        public UserSnapshot OwnerSnapshot { get; set; } = new();
        public PropertySnapshot PropertySnapshot { get; set; } = new();

        public static ContractSnapshot Create(ApplicationUser renter, ApplicationUser owner, Property property)
        {
            return new ContractSnapshot
            {
                RenterSnapshot = new UserSnapshot
                {
                    Id = renter.Id,
                    FullName = $"{renter.FirstName} {renter.LastName}",
                    Email = renter.Email,
                    PhoneNumber = renter.PhoneNumber,
                    NationalIDNumber = renter.NationalIDNumber,
                    ArabicFullName = renter.ArabicFullName,
                    ArabicAddress = renter.ArabicAddress,
                    ProfileImage = renter.ProfileImage
                },
                OwnerSnapshot = new UserSnapshot
                {
                    Id = owner.Id,
                    FullName = $"{owner.FirstName} {owner.LastName}",
                    Email = owner.Email,
                    PhoneNumber = owner.PhoneNumber,
                    NationalIDNumber = owner.NationalIDNumber,
                    ArabicFullName = owner.ArabicFullName,
                    ArabicAddress = owner.ArabicAddress,
                    ProfileImage = owner.ProfileImage
                },
                PropertySnapshot = new PropertySnapshot
                {
                    Id = property.Id,
                    Title = property.Title,
                    Description = property.Description,
                    Address = property.Address,
                    Price = property.Price,
                    RentalUnit = property.RentalUnit.ToString(),
                    PropertyType = property.Type.ToString(),
                    Bedrooms = property.Bedrooms,
                    Bathrooms = property.Bathrooms,
                    Beds = property.Beds
                }
            };
        }
    }

    public class UserSnapshot
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? NationalIDNumber { get; set; }
        public string? ArabicFullName { get; set; }
        public string? ArabicAddress { get; set; }
        public string? ProfileImage { get; set; }
    }

    public class PropertySnapshot
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string RentalUnit { get; set; } = string.Empty;
        public string PropertyType { get; set; } = string.Empty;
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int Beds { get; set; }
    }
}
