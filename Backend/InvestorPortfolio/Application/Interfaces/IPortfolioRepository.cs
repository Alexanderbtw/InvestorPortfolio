using System.Linq.Expressions;
using Core.Entities;

namespace Application.Interfaces;

public interface IPortfolioRepository : IGenericRepository<Portfolio>
{
    // Task<Portfolio?> GetPortfolioByIdAsync(Guid id);
    Task<Portfolio?> GetPortfolioByIdAsync(Guid id, Expression<Func<Portfolio,object>>[]? includePaths = null, bool asNoTracking = true);
}