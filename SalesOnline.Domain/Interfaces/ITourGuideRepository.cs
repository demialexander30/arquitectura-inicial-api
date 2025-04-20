using SalesOnline.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesOnline.Domain.Interfaces
{
    public interface ITourGuideRepository : IBaseRepository<TourGuide>
    {
        Task<IEnumerable<TourGuide>> GetBySpecializationAsync(string specialization);
        Task<IEnumerable<TourGuide>> GetAvailableGuidesAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<TourGuide>> GetByLanguageAsync(string language);
    }
}
