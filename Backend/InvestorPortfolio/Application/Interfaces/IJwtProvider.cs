using Core.Entities.Auth;

namespace Application.Interfaces;

public interface IJwtProvider
{
    string GenerateToken(User user);
}