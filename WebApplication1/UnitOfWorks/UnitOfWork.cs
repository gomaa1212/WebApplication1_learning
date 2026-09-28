using Microsoft.EntityFrameworkCore.Storage;
using System.Runtime.CompilerServices;
using WebApplication1.Data;
using WebApplication1.SharedRepository.Impelementations;
using WebApplication1.SharedRepository.Interfaces;

namespace WebApplication1.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IGenericRepository<T> Repository<T>() where T : class
        {
            if (_repositories.ContainsKey(typeof(T)))
            {
                return _repositories[typeof(T)] as IGenericRepository<T>;
            }
            var repository = new GenericRepository<T>(_context);
            _repositories[typeof(T)] = repository;
            return repository;

        }

        public async Task RollbackTransactionAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }
    }
}
