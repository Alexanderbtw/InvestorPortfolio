using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InvestorAPI.Contracts.Investment;

public record BuyStockRequest(
    [Required] Guid AccountId,
    [Required] string Uid,
    [DefaultValue(1)] ulong Quantity = 1);