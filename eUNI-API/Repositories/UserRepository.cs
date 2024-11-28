using eUNI_API.Data;
using eUNI_API.Models.Entities.Auth;
using eUNI_API.Repositories.Interfaces;

namespace eUNI_API.Repositories;

public class UserRepository(AppDbContext context): IUserRepository
{
    private readonly AppDbContext _context = context;
    
    public async Task<User> GetUserById(Guid id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null || user.IsDeleted)
            throw new BadHttpRequestException("Could not find user");
        
        return user;
    }
}