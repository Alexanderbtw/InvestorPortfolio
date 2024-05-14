using System.ComponentModel.DataAnnotations;

namespace InvestorAPI.Contracts.Investment;

public record CreateAccountRequest(
    [Required] string Name);