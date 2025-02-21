using SalesOnline.Domain.Core;
using SalesOnline.Domain.Interfaces;
using SalesOnline.Infrastructure.Context;
using SalesOnline.Infrastructure.Repositories;

namespace SalesOnline.Infrastructure
{
    public class UnitOfWork<T> : IUnitOfWork<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _context;
        private IGenericRepository<T> _repository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            _repository = new GenericRepository<T>(_context);
        }

        public IGenericRepository<T> Repository => _repository;
    }
}
