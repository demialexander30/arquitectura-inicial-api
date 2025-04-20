using Microsoft.EntityFrameworkCore;
using SalesOnline.Domain.Entities;
using SalesOnline.Domain.Interfaces;
using SalesOnline.Infrastructure.Context;

namespace SalesOnline.Infrastructure.Repositories
{
    public class TourRepository : ITourRepository
    {
        private readonly ApplicationDbContext _context;

        public TourRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tour>> GetAllAsync()
        {
            return await _context.Tours
                .Where(x => !x.IsDeleted)
                .ToListAsync();
        }

        public async Task<Tour?> GetByIdAsync(int id)
        {
            return await _context.Tours
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task AddAsync(Tour entity)
        {
            entity.CreatedDate = DateTime.Now;
            await _context.Tours.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Tour entity)
        {
            var existingTour = await _context.Tours.FindAsync(entity.Id);
            if (existingTour == null) return false;

            entity.ModifiedDate = DateTime.Now;
            _context.Entry(existingTour).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var tour = await _context.Tours.FindAsync(id);
            if (tour == null) return false;

            tour.IsDeleted = true;
            tour.ModifiedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Tour>> GetAvailableToursAsync()
        {
            return await _context.Tours
                .Where(x => !x.IsDeleted && x.IsAvailable)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tour>> GetToursByDestinationAsync(string destination)
        {
            return await _context.Tours
                .Where(x => !x.IsDeleted && x.Destination.Contains(destination))
                .ToListAsync();
        }

        public async Task<IEnumerable<Tour>> GetToursByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            return await _context.Tours
                .Where(x => !x.IsDeleted && x.Price >= minPrice && x.Price <= maxPrice)
                .ToListAsync();
        }
    }
}
