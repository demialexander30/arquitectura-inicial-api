using Microsoft.EntityFrameworkCore;
using SalesOnline.Domain.Entities;
using SalesOnline.Domain.Interfaces;
using SalesOnline.Infrastructure.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesOnline.Infrastructure.Repositories
{
    public class TourGuideRepository : ITourGuideRepository
    {
        private readonly ApplicationDbContext _context;

        public TourGuideRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TourGuide>> GetAllAsync()
        {
            return await _context.TourGuides
                .Include(x => x.Tours)
                .Where(x => !x.IsDeleted)
                .ToListAsync();
        }

        public async Task<TourGuide?> GetByIdAsync(int id)
        {
            return await _context.TourGuides
                .Include(x => x.Tours)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task AddAsync(TourGuide entity)
        {
            entity.CreatedDate = DateTime.Now;
            await _context.TourGuides.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(TourGuide entity)
        {
            var existingGuide = await _context.TourGuides.FindAsync(entity.Id);
            if (existingGuide == null) return false;

            entity.ModifiedDate = DateTime.Now;
            _context.Entry(existingGuide).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var guide = await _context.TourGuides.FindAsync(id);
            if (guide == null) return false;

            guide.IsDeleted = true;
            guide.ModifiedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<TourGuide>> GetBySpecializationAsync(string specialization)
        {
            return await _context.TourGuides
                .Include(x => x.Tours)
                .Where(x => x.Specialization == specialization && !x.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<TourGuide>> GetAvailableGuidesAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.TourGuides
                .Include(x => x.Tours)
                .Where(x => !x.IsDeleted &&
                           !x.Tours.Any(t => 
                               (t.StartDate <= endDate && t.EndDate >= startDate) && 
                               !t.IsDeleted))
                .ToListAsync();
        }

        public async Task<IEnumerable<TourGuide>> GetByLanguageAsync(string language)
        {
            return await _context.TourGuides
                .Include(x => x.Tours)
                .Where(x => x.Languages.Contains(language) && !x.IsDeleted)
                .ToListAsync();
        }
    }
}
