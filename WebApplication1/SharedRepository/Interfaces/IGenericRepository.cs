using Microsoft.EntityFrameworkCore.Storage;

namespace WebApplication1.SharedRepository.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<T> GetByIdAsync(int id);
        public IQueryable<T> GetAsQueryable();
        public Task<List<T>> GetAllListAsync();
        public Task AddAsync(T entity);
        public Task UpdateAsync(T entity);
        public Task DeleteAsync(T entity);
        public Task AddRangeAsync(IEnumerable<T> entities);
        public Task<IDbContextTransaction> BeginTransactionAsync();

    }
}
