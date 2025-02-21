using SalesOnline.Domain.Core;

namespace SalesOnline.Domain.Interfaces
{
    public interface IUnitOfWork<T> where T : BaseEntity
    {
        IGenericRepository<T> Repository { get; }
    }
}
