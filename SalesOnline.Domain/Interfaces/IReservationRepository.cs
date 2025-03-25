using SalesOnline.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesOnline.Domain.Interfaces
{
    public interface IReservationRepository : IGenericRepository<Reservation>
    {
        Task<IEnumerable<Reservation>> GetByCustomerId(int customerId);
        Task<IEnumerable<Reservation>> GetByTourId(int tourId);
        Task<IEnumerable<Reservation>> GetByStatus(ReservationStatus status);
        Task<IEnumerable<Reservation>> GetByDateRange(DateTime startDate, DateTime endDate);
    }
}
