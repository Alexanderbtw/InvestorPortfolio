using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interfaces;
using Core.Entities.Auth;
using Infrastructure.DTOs;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Auth;

public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
{
    private readonly JwtOptions _options = options.Value;

    public string GenerateToken(User user)
    {
        Claim[] claims = [new Claim("userId", user.Id.ToString())];
        
        var signingCredentials = new SigningCredentials(
            key:       new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
            algorithm: SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow + _options.Lifetime
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}