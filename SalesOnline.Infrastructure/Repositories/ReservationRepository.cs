using Microsoft.EntityFrameworkCore;
using SalesOnline.Domain.Context;
using SalesOnline.Domain.Entities;
using SalesOnline.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesOnline.Infrastructure.Repositories
{
    public class ReservationRepository : GenericRepository<Reservation>, IReservationRepository
    {
        public ReservationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Reservation>> GetByCustomerId(int customerId)
        {
            return await _context.Set<Reservation>()
                .Include(r => r.Tour)
                .Where(r => r.CustomerId == customerId && !r.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetByTourId(int tourId)
        {
            return await _context.Set<Reservation>()
                .Include(r => r.Customer)
                .Where(r => r.TourId == tourId && !r.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetByStatus(ReservationStatus status)
        {
            return await _context.Set<Reservation>()
                .Include(r => r.Tour)
                .Include(r => r.Customer)
                .Where(r => r.Status == status && !r.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetByDateRange(DateTime startDate, DateTime endDate)
        {
            return await _context.Set<Reservation>()
                .Include(r => r.Tour)
                .Include(r => r.Customer)
                .Where(r => r.ReservationDate >= startDate && r.ReservationDate <= endDate && !r.IsDeleted)
                .ToListAsync();
        }
    }
}
