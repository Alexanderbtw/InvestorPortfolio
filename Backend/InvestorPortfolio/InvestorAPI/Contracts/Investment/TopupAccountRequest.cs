using System.ComponentModel.DataAnnotations;
using Core.Entities.SpecificData;

namespace InvestorAPI.Contracts.Investment;

public record TopupAccountRequest(
    [Required] Guid AccountId,
    [Required] MoneyValue Value);