using SalesOnline.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesOnline.Domain.Interfaces
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        Task<IEnumerable<Review>> GetByTourId(int tourId);
        Task<IEnumerable<Review>> GetByCustomerId(int customerId);
        Task<double> GetAverageRatingForTour(int tourId);
        Task<IEnumerable<Review>> GetRecentReviews(int count);
    }
}
