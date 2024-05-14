using Core.Entities;

namespace InvestorAPI.Contracts.Investment;

public record CreateAccountResponse(
    BrokerageAccount Account);