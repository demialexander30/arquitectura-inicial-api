using SalesOnline.Domain.Entities;

namespace SalesOnline.Domain.Interfaces
{
    public interface ITourRepository : IGenericRepository<Tour>
    {
        // Métodos específicos para Tour si son necesarios
        Task<IEnumerable<Tour>> GetAvailableToursAsync();
        Task<IEnumerable<Tour>> GetToursByDestinationAsync(string destination);
        Task<IEnumerable<Tour>> GetToursByPriceRangeAsync(decimal minPrice, decimal maxPrice);
    }
}
