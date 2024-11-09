using eUNI_API.Data;
using eUNI_API.Models.Dto;
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

    public async Task<User> GetUserById(Guid id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null || user.IsDeleted)
            throw new BadHttpRequestException("Could not find user");
        
        return user;
    }

    public async Task UpdateUser(User user, UpdateUserDto updateUserDto)
    {
        foreach (var propertyInfo in updateUserDto.GetType().GetProperties())
        {
            var value = propertyInfo.GetValue(updateUserDto);
            if (value is not string stringValue || string.IsNullOrEmpty(stringValue)) continue;
            
            user.GetType().GetProperty(propertyInfo.Name)?.SetValue(user, value);
        }
        
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}