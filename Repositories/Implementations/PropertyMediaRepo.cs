using MARN_API.Data;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MARN_API.Repositories.Implementations
{
    public class PropertyMediaRepo : IPropertyMediaRepo
    {
        private readonly AppDbContext _context;

        public PropertyMediaRepo(AppDbContext context)
        {
            _context = context;
        }


        public async Task<List<PropertyMedia>> GetByPropertyIdAsync(long propertyId)
        {
            return await _context.PropertyMedia
                .Where(m => m.PropertyId == propertyId)
                .ToListAsync();
        }

        public async Task AddByPropertyIdAsync(long propertyId, PropertyMedia media)
        {
            media.PropertyId = propertyId;
            await _context.PropertyMedia.AddAsync(media);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveByObjectIdAsync(long id)
        {
            var media = await _context.PropertyMedia.FindAsync(id);
            if (media != null)
            {
                _context.PropertyMedia.Remove(media);
                await _context.SaveChangesAsync();
            }
        }
    }
}
