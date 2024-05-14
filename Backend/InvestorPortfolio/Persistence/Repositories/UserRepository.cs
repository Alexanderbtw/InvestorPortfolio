using Application.Interfaces;
using Core.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Persistence.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(InvestmentDbContext context, ILoggerFactory loggerFactory) : 
        base(context, loggerFactory.CreateLogger<UserRepository>())
    { }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await DbSet.FirstOrDefaultAsync(u => u.Email == email);
    }
}