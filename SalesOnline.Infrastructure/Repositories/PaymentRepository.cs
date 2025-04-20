using Microsoft.EntityFrameworkCore;
using SalesOnline.Domain.Entities;
using SalesOnline.Domain.Interfaces;
using SalesOnline.Infrastructure.Context;
using SalesOnline.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesOnline.Infrastructure.Repositories
{
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Payment>> GetByReservationIdAsync(int reservationId)
        {
            return await _entities
                .Where(p => p.ReservationId == reservationId && !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _entities
                .Where(p => p.PaymentDate >= startDate && 
                           p.PaymentDate <= endDate && 
                           !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalPaymentsForPeriodAsync(DateTime startDate, DateTime endDate)
        {
            return await _entities
                .Where(p => p.PaymentDate >= startDate && 
                           p.PaymentDate <= endDate && 
                           !p.IsDeleted)
                .SumAsync(p => p.Amount);
        }

        public async Task<IEnumerable<Payment>> GetPendingPaymentsAsync()
        {
            return await _entities
                .Where(p => p.PaymentStatus == "Pending" && !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByMethodAsync(string paymentMethod)
        {
            return await _entities
                .Where(p => p.PaymentMethod == paymentMethod && !p.IsDeleted)
                .ToListAsync();
        }
    }
}
