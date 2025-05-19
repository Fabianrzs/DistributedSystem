using System.Linq.Expressions;
using Domain.Abstractions.Entities;
using Infrastructure.Implementations.Persistence.EFCore.Contexts;
using Microsoft.EntityFrameworkCore;
using Template.Domain.Abstractions.Repositories;

namespace Infrastructure.Implementations.Persistence.EFCore.Repositories;

public class Repository<TEntity>(ApplicationsDbContext context) : IRepository<TEntity>
    where TEntity : Entity
{
    #region Métodos CUD
    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await context.Set<TEntity>().AnyAsync(predicate);
    }

    public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await context.Set<TEntity>().AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    public virtual async Task UpdateRangeAsync(IEnumerable<TEntity> entities)
    {
        context.Set<TEntity>().UpdateRange(entities);
        await context.SaveChangesAsync();
    }

    public virtual async Task DeleteRangeAsync(IEnumerable<Guid> ids)
    {
        ICollection<TEntity> entities = [];

        foreach (Guid id in ids)
        {
            TEntity entity = await context.Set<TEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id && e.IsActive);
            entity!.InActive();
            entities.Add(entity);
        }
        
        context.Attach(entities);
        context.Set<TEntity>().UpdateRange(entities);
        await context.SaveChangesAsync();
    }

    public virtual async Task AddAsync(TEntity entity)
    {
        await context.Set<TEntity>().AddAsync(entity);
        await context.SaveChangesAsync();

    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        context.Set<TEntity>().Update(entity);
        await context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        TEntity entity = await context.Set<TEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id && e.IsActive)
            ?? throw new KeyNotFoundException($"No se encontró la entidad con ID {id}");

        entity.InActive();
        context.Attach(entity);
        context.Set<TEntity>().Update(entity);
        await context.SaveChangesAsync();
    }

    #endregion

    #region Métodos de consulta

    public virtual async Task<IEnumerable<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = context.Set<TEntity>().AsNoTracking().Where(predicate);
        query = ApplyIncludes(query, includes);
        return await query.ToListAsync();
    }

    public virtual async Task<TEntity> FindFirstAsync(
        Expression<Func<TEntity, bool>> predicate,
        params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = context.Set<TEntity>().AsNoTracking().Where(predicate);
        query = ApplyIncludes(query, includes);
        return await query.FirstOrDefaultAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(
        params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = context.Set<TEntity>().AsNoTracking();
        query = ApplyIncludes(query, includes);
        return await query.ToListAsync();
    }

    public virtual async Task<TEntity?> GetByIdAsync(Guid id, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = context.Set<TEntity>().AsNoTracking().Where(e => e.Id == id);
        query = ApplyIncludes(query, includes);
        return await query.FirstOrDefaultAsync();
    }

    public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await context.Set<TEntity>().CountAsync(predicate);
    }

    #endregion

    #region Métodos privados

    private static IQueryable<TEntity> ApplyIncludes(
        IQueryable<TEntity> query,
        Expression<Func<TEntity, object>>[] includes)
    {
        foreach (Expression<Func<TEntity, object>> include in includes)
        {
            query = query.Include(include);
        }
        return query;
    }

    #endregion
}
