using MARN_API.Data;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MARN_API.Repositories.Implementations
{
    public class PropertyRuleRepo : IPropertyRuleRepo
    {
        private readonly AppDbContext _context;

        public PropertyRuleRepo(AppDbContext context)
        {
            _context = context;
        }


        public async Task<List<PropertyRule>> GetByPropertyIdAsync(long propertyId)
        {
            return await _context.PropertyRules
                .Where(r => r.PropertyId == propertyId)
                .ToListAsync();
        }

        public async Task AddByPropertyIdAsync(long propertyId, PropertyRule rule)
        {
            rule.PropertyId = propertyId;
            await _context.PropertyRules.AddAsync(rule);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveByObjectIdAsync(long id)
        {
            var rule = await _context.PropertyRules.FindAsync(id);
            if (rule != null)
            {
                _context.PropertyRules.Remove(rule);
                await _context.SaveChangesAsync();
            }
        }
    }
}
