using eUNI_API.Data;
using eUNI_API.Enums;
using eUNI_API.Exception;
using eUNI_API.Helpers;
using eUNI_API.Models.Entities.Auth;
using eUNI_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Repositories;

public class UserRepository(AppDbContext context): IUserRepository
{
    private readonly AppDbContext _context = context;

    public User? GetUserByEmail(string email)
    {
        return _context.Users
            .AsNoTracking()
            .FirstOrDefault(u => u.Email == email.ToLower());
    }

    public void RemoveUserByEmail(string email)
    {
        var user = GetUserByEmail(email);
        if(user == null) return;
        _context.Users.Remove(user);
        _context.SaveChanges();
    }
    
    public async Task<User> CreateUser(string firstName, string lastName, string email, string password, int roleId)
    {
        if(GetUserByEmail(email) != null) throw new HttpForbiddenException("Email is already taken");
        var salt = PasswordHasher.GenerateSalt();
        var user = new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email.ToLower(),
            RoleId = roleId,
            Salt = salt,
            PasswordHash = PasswordHasher.HashPassword(password, salt)
        };

        await _context.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> GetUserById(Guid id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null || user.IsDeleted)
            throw new BadHttpRequestException("Could not find user");
        
        return user;
    }

    public void DeleteAllUsers()
    {
        _context.Users.RemoveRange(_context.Users);
        _context.SaveChanges();
    }
}