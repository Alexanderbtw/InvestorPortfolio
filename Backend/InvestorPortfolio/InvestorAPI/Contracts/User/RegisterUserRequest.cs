using System.ComponentModel.DataAnnotations;

namespace InvestorAPI.Contracts.User;

public record RegisterUserRequest(
    [Required] string Username,
    [Required] string Email,
    [Required] string Password);