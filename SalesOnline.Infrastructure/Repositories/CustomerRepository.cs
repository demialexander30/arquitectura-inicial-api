using Microsoft.EntityFrameworkCore;
using SalesOnline.Domain.Context;
using SalesOnline.Domain.Entities;
using SalesOnline.Domain.Interfaces;

namespace SalesOnline.Infrastructure.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Customer>> GetByNationality(string nationality)
        {
            return await _context.Set<Customer>()
                .Where(c => c.Nationality == nationality && !c.IsDeleted)
                .ToListAsync();
        }

        public async Task<Customer> GetByPassportNumber(string passportNumber)
        {
            return await _context.Set<Customer>()
                .FirstOrDefaultAsync(c => c.PassportNumber == passportNumber && !c.IsDeleted);
        }
    }
}
