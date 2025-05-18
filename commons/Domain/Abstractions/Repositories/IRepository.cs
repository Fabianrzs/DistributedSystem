using System.Linq.Expressions;
using Domain.Abstractions.Entities;

namespace Template.Domain.Abstractions.Repositories;

public interface IRepository<TEntity> where TEntity : Entity
{
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> GetByIdAsync(Guid id, params Expression<Func<TEntity, object>>[] includes);
    Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includes);
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);
    Task<TEntity> FindFirstAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);
    Task AddRangeAsync(IEnumerable<TEntity> entities);
    Task AddAsync(TEntity entity);
    Task UpdateRangeAsync(IEnumerable<TEntity> entities);
    Task UpdateAsync(TEntity entity);
    Task DeleteRangeAsync(IEnumerable<Guid> ids);
    Task DeleteAsync(Guid id);
    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
}
