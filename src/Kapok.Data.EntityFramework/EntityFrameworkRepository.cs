using System.Data.Entity;

namespace Kapok.Data.EntityFramework;

public sealed class EntityFrameworkRepository<T> : IRepository<T>
    where T : class
{
    //private readonly DbChangeTracker _changeTracker;
    private readonly DbSet<T> _dbSet;

    internal EntityFrameworkRepository(EntityFrameworkDataDomainScope dataDomainScope)
    {
        _dbSet = dataDomainScope.DbContext.Set<T>();
        //_changeTracker = dataDomainScope.DbContext.ChangeTracker;
    }

    // TODO: to be implemented
    public ICollection<string>? IncludeNestedData => null;
        
    private IQueryable<T> AddNestedData(IQueryable<T> queryable)
    {
        // nested loading
        if (IncludeNestedData != null && IncludeNestedData.Count > 0)
        {
            foreach (var navigationPropertyPath in IncludeNestedData)
            {
                queryable = queryable.Include(navigationPropertyPath);
            }
        }

        return queryable;
    }

    public IQueryable<T> AsQueryable()
    {
        IQueryable<T> queryable = _dbSet.AsQueryable();

        // nested loading
        queryable = AddNestedData(queryable);

        return queryable;
    }

    public void Create(T entity)
    {
        _dbSet.Add(entity);
    }

    public void Update(T entity, T? originalEntity)
    {
        throw new NotImplementedException();
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void CreateRange(IEnumerable<T> entities)
    {
        _dbSet.AddRange(entities);
    }

    public void UpdateRange(IEnumerable<T> entities)
    {
        throw new NotImplementedException();
    }

    public void DeleteRange(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
    }

    public Task CreateAsync(T entity)
    {
        Create(entity);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(T entity, T? originalEntity)
    {
        Update(entity, originalEntity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity)
    {
        Delete(entity);
        return Task.CompletedTask;
    }

    public Task CreateRangeAsync(IEnumerable<T> entities)
    {
        CreateRange(entities);
        return Task.CompletedTask;
    }

    public Task UpdateRangeAsync(IEnumerable<T> entities)
    {
        UpdateRange(entities);
        return Task.CompletedTask;
    }

    public Task DeleteRangeAsync(IEnumerable<T> entities)
    {
        DeleteRange(entities);
        return Task.CompletedTask;
    }
}