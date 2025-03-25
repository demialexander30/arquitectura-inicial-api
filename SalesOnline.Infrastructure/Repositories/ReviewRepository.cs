using Microsoft.EntityFrameworkCore;
using SalesOnline.Domain.Context;
using SalesOnline.Domain.Entities;
using SalesOnline.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesOnline.Infrastructure.Repositories
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Review>> GetByTourId(int tourId)
        {
            return await _context.Set<Review>()
                .Include(r => r.Customer)
                .Where(r => r.TourId == tourId && !r.IsDeleted)
                .OrderByDescending(r => r.ReviewDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetByCustomerId(int customerId)
        {
            return await _context.Set<Review>()
                .Include(r => r.Tour)
                .Where(r => r.CustomerId == customerId && !r.IsDeleted)
                .OrderByDescending(r => r.ReviewDate)
                .ToListAsync();
        }

        public async Task<double> GetAverageRatingForTour(int tourId)
        {
            return await _context.Set<Review>()
                .Where(r => r.TourId == tourId && !r.IsDeleted)
                .AverageAsync(r => r.Rating);
        }

        public async Task<IEnumerable<Review>> GetRecentReviews(int count)
        {
            return await _context.Set<Review>()
                .Include(r => r.Tour)
                .Include(r => r.Customer)
                .Where(r => !r.IsDeleted)
                .OrderByDescending(r => r.ReviewDate)
                .Take(count)
                .ToListAsync();
        }
    }
}
