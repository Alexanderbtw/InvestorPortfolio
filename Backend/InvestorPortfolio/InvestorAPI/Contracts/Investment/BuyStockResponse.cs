using Core.Entities.Base;

namespace InvestorAPI.Contracts.Investment;

public record BuyStockResponse<T>(
    T Value) where T : Stock;
