using SalesOnline.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesOnline.Domain.Interfaces
{
    public interface IReviewRepository : IBaseRepository<Review>
    {
        Task<IEnumerable<Review>> GetByTourIdAsync(int tourId);
        Task<IEnumerable<Review>> GetByCustomerIdAsync(int customerId);
        Task<double> GetAverageRatingForTourAsync(int tourId);
        Task<IEnumerable<Review>> GetRecentReviewsAsync(int count);
    }
}
