using Microsoft.EntityFrameworkCore;
using SalesOnline.Domain.Entities;
using SalesOnline.Domain.Interfaces;
using SalesOnline.Infrastructure.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesOnline.Infrastructure.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Review>> GetAllAsync()
        {
            return await _context.Reviews
                .Include(x => x.Customer)
                .Include(x => x.Tour)
                .Where(x => !x.IsDeleted)
                .ToListAsync();
        }

        public async Task<Review?> GetByIdAsync(int id)
        {
            return await _context.Reviews
                .Include(x => x.Customer)
                .Include(x => x.Tour)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task AddAsync(Review entity)
        {
            entity.CreatedDate = DateTime.Now;
            await _context.Reviews.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Review entity)
        {
            var existingReview = await _context.Reviews.FindAsync(entity.Id);
            if (existingReview == null) return false;

            entity.ModifiedDate = DateTime.Now;
            _context.Entry(existingReview).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return false;

            review.IsDeleted = true;
            review.ModifiedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Review>> GetByTourIdAsync(int tourId)
        {
            return await _context.Reviews
                .Include(x => x.Customer)
                .Include(x => x.Tour)
                .Where(x => x.TourId == tourId && !x.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetByCustomerIdAsync(int customerId)
        {
            return await _context.Reviews
                .Include(x => x.Customer)
                .Include(x => x.Tour)
                .Where(x => x.CustomerId == customerId && !x.IsDeleted)
                .ToListAsync();
        }

        public async Task<double> GetAverageRatingForTourAsync(int tourId)
        {
            return await _context.Reviews
                .Where(x => x.TourId == tourId && !x.IsDeleted)
                .AverageAsync(x => x.Rating);
        }

        public async Task<IEnumerable<Review>> GetRecentReviewsAsync(int count)
        {
            return await _context.Reviews
                .Include(x => x.Tour)
                .Include(x => x.Customer)
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.ReviewDate)
                .Take(count)
                .ToListAsync();
        }
    }
}
