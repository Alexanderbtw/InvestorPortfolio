using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Application.Interfaces;
using Core.Entities;
using Core.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Persistence.Repositories;

public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly DbSet<T> DbSet;
    protected readonly ILogger Logger;

    protected GenericRepository(InvestmentDbContext context, ILogger logger)
    {
        Logger = logger;
        DbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[]? includePaths = null, bool asNoTracking = true)
    {
        var res = DbSet.AsQueryable();
        if (asNoTracking)
        {
            res = DbSet.AsNoTracking();
        }

        if (includePaths is not null)
        {
            res = includePaths.Aggregate(res, (current, path) => current.Include(path));
        }
        
        return await res.FirstOrDefaultAsync(predicate);
    }

    public virtual async Task<bool> TryAddAsync(T entity)
    {
        try
        {
            var res = await DbSet.AddAsync(entity);
            return res.State == EntityState.Added;
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Failed to add {type}", typeof(T));
            return false;
        }
    }

    public virtual bool Remove(T entity)
    {
        try
        {
            var res = DbSet.Remove(entity);
            return res.State == EntityState.Deleted;
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Failed to add {type}", typeof(T));
            return false;
        }
    }

    public virtual async Task<int> DeleteAsync(Expression<Func<T, bool>> expression)
    { 
        return await DbSet.Where(expression).ExecuteDeleteAsync();
    }
}