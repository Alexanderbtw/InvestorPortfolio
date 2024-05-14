using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Application.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[]? includePaths = null, bool asNoTracking = true);
    Task<bool> TryAddAsync(T entity);
    bool Remove(T entity);
    Task<int> DeleteAsync(Expression<Func<T, bool>> expression);
}