using MARN_API.Data;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MARN_API.Repositories.Implementations
{
    public class PropertyAmenityRepo : IPropertyAmenityRepo
    {
        private readonly AppDbContext _context;

        public PropertyAmenityRepo(AppDbContext context)
        {
            _context = context;
        }


        public async Task<List<PropertyAmenity>> GetByPropertyIdAsync(long propertyId)
        {
            return await _context.PropertyAmenities
                .Where(a => a.PropertyId == propertyId)
                .ToListAsync();
        }

        public async Task AddByPropertyIdAsync(long propertyId, PropertyAmenity amenity)
        {
            amenity.PropertyId = propertyId;
            await _context.PropertyAmenities.AddAsync(amenity);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveByObjectIdAsync(long id)
        {
            var amenity = await _context.PropertyAmenities.FindAsync(id);
            if (amenity != null)
            {
                _context.PropertyAmenities.Remove(amenity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
