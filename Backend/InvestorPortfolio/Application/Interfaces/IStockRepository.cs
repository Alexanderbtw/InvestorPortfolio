using Core.Entities;
using Core.Entities.Base;

namespace Application.Interfaces;

public interface IStockRepository<T> :  IGenericRepository<StockContainer<T>> where T : Stock
{
    void UpdateRange(IEnumerable<StockContainer<T>> stocks);
    void Update(StockContainer<T> stock);
}