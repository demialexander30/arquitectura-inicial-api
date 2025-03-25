using SalesOnline.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesOnline.Domain.Interfaces
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<IEnumerable<Payment>> GetByReservationId(int reservationId);
        Task<IEnumerable<Payment>> GetByStatus(PaymentStatus status);
        Task<IEnumerable<Payment>> GetByDateRange(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Payment>> GetByPaymentMethod(PaymentMethod method);
    }
}
