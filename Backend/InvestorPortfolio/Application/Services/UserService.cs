using Application.Interfaces;
using Core.Entities.Auth;

namespace Application.Services;

public class UserService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
{

    public async Task Register(string username, string email, string password)
    {
        var hashedPassword = passwordHasher.Hash(password);
        
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = username,
            PasswordHash = hashedPassword,
            Email = email
        };
        
        await unitOfWork.TryAddUserWithPortfolioAsync(user);
        await unitOfWork.CompleteAsync();
    }

    public async Task<string> Login(string email, string password)
    {
        var user = await unitOfWork.Users.GetAsync(u => u.Email == email);

        if (user is null)
            throw new NullReferenceException("User not found");
        if (!passwordHasher.Verify(password, user.PasswordHash))
            throw new ArgumentException("Password doesn't match");
        
        return jwtProvider.GenerateToken(user);
    }
}