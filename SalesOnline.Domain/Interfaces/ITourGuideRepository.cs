using SalesOnline.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesOnline.Domain.Interfaces
{
    public interface ITourGuideRepository : IGenericRepository<TourGuide>
    {
        Task<IEnumerable<TourGuide>> GetByLanguage(string language);
        Task<IEnumerable<TourGuide>> GetBySpecialization(string specialization);
        Task<IEnumerable<TourGuide>> GetAvailableForDateRange(DateTime startDate, DateTime endDate);
    }
}
