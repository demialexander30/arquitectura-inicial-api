using Microsoft.EntityFrameworkCore;
using SalesOnline.Domain.Entities;
using SalesOnline.Domain.Interfaces;
using SalesOnline.Infrastructure.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesOnline.Infrastructure.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly ApplicationDbContext _context;

        public ReservationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Reservation>> GetAllAsync()
        {
            return await _context.Reservations
                .Include(x => x.Customer)
                .Include(x => x.Tour)
                .Where(x => !x.IsDeleted)
                .ToListAsync();
        }

        public async Task<Reservation?> GetByIdAsync(int id)
        {
            return await _context.Reservations
                .Include(x => x.Customer)
                .Include(x => x.Tour)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task AddAsync(Reservation entity)
        {
            entity.CreatedDate = DateTime.Now;
            await _context.Reservations.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Reservation entity)
        {
            var existingReservation = await _context.Reservations.FindAsync(entity.Id);
            if (existingReservation == null) return false;

            entity.ModifiedDate = DateTime.Now;
            _context.Entry(existingReservation).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null) return false;

            reservation.IsDeleted = true;
            reservation.ModifiedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Reservation>> GetByCustomerIdAsync(int customerId)
        {
            return await _context.Reservations
                .Include(x => x.Customer)
                .Include(x => x.Tour)
                .Where(x => x.CustomerId == customerId && !x.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetByTourIdAsync(int tourId)
        {
            return await _context.Reservations
                .Include(x => x.Customer)
                .Include(x => x.Tour)
                .Where(x => x.TourId == tourId && !x.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Reservations
                .Include(x => x.Customer)
                .Include(x => x.Tour)
                .Where(x => x.ReservationDate >= startDate && 
                           x.ReservationDate <= endDate && 
                           !x.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetByStatusAsync(ReservationStatus status)
        {
            return await _context.Reservations
                .Include(x => x.Customer)
                .Include(x => x.Tour)
                .Where(x => x.Status == status && !x.IsDeleted)
                .ToListAsync();
        }
    }
}
