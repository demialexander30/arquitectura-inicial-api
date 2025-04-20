using Microsoft.EntityFrameworkCore;
using SalesOnline.Domain.Entities;
using SalesOnline.Domain.Interfaces;
using SalesOnline.Infrastructure.Context;

namespace SalesOnline.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _context.Customers
                .Where(x => !x.IsDeleted)
                .ToListAsync();
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task AddAsync(Customer entity)
        {
            entity.CreatedDate = DateTime.Now;
            await _context.Customers.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Customer entity)
        {
            var existingCustomer = await _context.Customers.FindAsync(entity.Id);
            if (existingCustomer == null) return false;

            entity.ModifiedDate = DateTime.Now;
            _context.Entry(existingCustomer).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return false;

            customer.IsDeleted = true;
            customer.ModifiedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Customer?> GetByEmailAsync(string email)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(x => x.Email == email && !x.IsDeleted);
        }

        public async Task<IEnumerable<Customer>> GetByNationality(string nationality)
        {
            return await _context.Customers
                .Where(x => x.Nationality == nationality && !x.IsDeleted)
                .ToListAsync();
        }

        public async Task<Customer?> GetByPassportNumber(string passportNumber)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(x => x.PassportNumber == passportNumber && !x.IsDeleted);
        }
    }
}
