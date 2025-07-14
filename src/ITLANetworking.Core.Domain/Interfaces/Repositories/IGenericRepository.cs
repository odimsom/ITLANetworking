using System.Linq.Expressions;

namespace ITLANetworking.Core.Domain.Interfaces.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity, int id);
        Task DeleteAsync(TEntity entity);
        Task<TEntity?> GetByIdAsync(int id);
        Task<List<TEntity>> GetAllAsync();
        Task<List<TEntity>> GetAllWithIncludeAsync(List<string> properties);
        Task<TEntity?> GetByIdWithIncludeAsync(int id, List<string> properties);
        Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> FindWithIncludeAsync(Expression<Func<TEntity, bool>> predicate, List<string> properties);
    }
}
