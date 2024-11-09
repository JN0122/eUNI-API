using eUNI_API.Data;
using eUNI_API.Models.Entities.User;
using eUNI_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Services;

public class UsersService(AppDbContext context): IUsersService
{
    private readonly AppDbContext _context = context;
    
    public async Task<List<User>> GetUsers()
    {
        return await _context.Users.Where(u => !u.IsDeleted).ToListAsync();
    }

    public Task RemoveUser(Guid id)
    {
        var user = _context.Users.Find(id);
        if (user == null)
            throw new BadHttpRequestException("Could not find user");
        
        user.IsDeleted = true;
        context.Users.Update(user);
        return context.SaveChangesAsync();
    }
}