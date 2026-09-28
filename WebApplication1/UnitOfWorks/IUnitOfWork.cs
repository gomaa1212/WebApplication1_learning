using Microsoft.EntityFrameworkCore.Storage;
using WebApplication1.SharedRepository.Interfaces;

namespace WebApplication1.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        public IGenericRepository<T> Repository<T>() where T : class;
        public Task<IDbContextTransaction> BeginTransactionAsync();
        public Task CommitTransactionAsync();
        public Task RollbackTransactionAsync();
        public Task<int> CompleteAsync();
    }
}
