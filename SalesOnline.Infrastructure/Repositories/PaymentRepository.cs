using Microsoft.EntityFrameworkCore;
using SalesOnline.Domain.Context;
using SalesOnline.Domain.Entities;
using SalesOnline.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesOnline.Infrastructure.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Payment>> GetByReservationId(int reservationId)
        {
            return await _context.Set<Payment>()
                .Include(p => p.Reservation)
                .Where(p => p.ReservationId == reservationId && !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetByStatus(PaymentStatus status)
        {
            return await _context.Set<Payment>()
                .Include(p => p.Reservation)
                .Where(p => p.Status == status && !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetByDateRange(DateTime startDate, DateTime endDate)
        {
            return await _context.Set<Payment>()
                .Include(p => p.Reservation)
                .Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate && !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetByPaymentMethod(PaymentMethod method)
        {
            return await _context.Set<Payment>()
                .Include(p => p.Reservation)
                .Where(p => p.Method == method && !p.IsDeleted)
                .ToListAsync();
        }
    }
}
