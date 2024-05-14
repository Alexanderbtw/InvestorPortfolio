namespace Core.Entities.Auth;

public class User
{
    public required Guid Id { get; init; }
    public required string UserName { get; init; }
    public required string PasswordHash { get; init; }
    public required string Email { get; init; }
    public byte[]? Version { get; init; }
    // public string? Role { get; set; }
}