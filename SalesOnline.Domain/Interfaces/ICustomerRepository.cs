using SalesOnline.Domain.Core;
using SalesOnline.Domain.Entities;

namespace SalesOnline.Domain.Interfaces
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<IEnumerable<Customer>> GetByNationality(string nationality);
        Task<Customer> GetByPassportNumber(string passportNumber);
    }
}
