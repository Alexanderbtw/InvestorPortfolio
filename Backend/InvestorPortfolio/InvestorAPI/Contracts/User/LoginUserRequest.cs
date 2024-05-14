using System.ComponentModel.DataAnnotations;

namespace InvestorAPI.Contracts.User;

public record LoginUserRequest(
    [Required] string Email,
    [Required] string Password);