using Microsoft.EntityFrameworkCore;
using SalesOnline.Domain.Context;
using SalesOnline.Domain.Entities;
using SalesOnline.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesOnline.Infrastructure.Repositories
{
    public class TourGuideRepository : GenericRepository<TourGuide>, ITourGuideRepository
    {
        public TourGuideRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TourGuide>> GetByLanguage(string language)
        {
            return await _context.Set<TourGuide>()
                .Where(g => g.Languages.Contains(language) && !g.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<TourGuide>> GetBySpecialization(string specialization)
        {
            return await _context.Set<TourGuide>()
                .Where(g => g.Specialization == specialization && !g.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<TourGuide>> GetAvailableForDateRange(DateTime startDate, DateTime endDate)
        {
            return await _context.Set<TourGuide>()
                .Where(g => !g.Tours.Any(t => 
                    (t.StartDate <= endDate && t.EndDate >= startDate) && !g.IsDeleted))
                .ToListAsync();
        }
    }
}
