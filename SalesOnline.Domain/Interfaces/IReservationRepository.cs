using SalesOnline.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesOnline.Domain.Interfaces
{
    public interface IReservationRepository : IBaseRepository<Reservation>
    {
        Task<IEnumerable<Reservation>> GetByCustomerIdAsync(int customerId);
        Task<IEnumerable<Reservation>> GetByTourIdAsync(int tourId);
        Task<IEnumerable<Reservation>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Reservation>> GetByStatusAsync(ReservationStatus status);
    }
}
