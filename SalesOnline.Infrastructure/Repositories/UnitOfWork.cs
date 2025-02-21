using SalesOnline.Domain.Core;
using SalesOnline.Domain.Interfaces;
using SalesOnline.Infrastructure.Context;

namespace SalesOnline.Infrastructure.Repositories
{
    public class UnitOfWork<T> : IUnitOfWork<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _context;
        private IGenericRepository<T> _repository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<T> Repository
        {
            get
            {
                if (_repository == null)
                {
                    _repository = new GenericRepository<T>(_context);
                }
                return _repository;
            }
        }
    }
}
