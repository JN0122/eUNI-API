using eUNI_API.Data;
using eUNI_API.Enums;
using eUNI_API.Models.Dto.Auth;
using eUNI_API.Models.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Services;

public class UserService(AppDbContext context): IUserService
{
    private readonly AppDbContext _context = context;

    public async Task<User> CreateUser(UserCreate user)
    {
        var studentRole = _context.Roles.FirstOrDefault(role => role.Id == (int)UserRole.Student);

        if (studentRole == null)
            throw new Exception("Student role doesn't exist");

        var newUser = new User
        {
            Email = user.Email,
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            PasswordHash = user.PasswordHash,
            Salt = user.Salt,
            Role = studentRole,
        };
        
        await _context.Users.AddAsync(newUser);
        _context.SaveChanges();
        return newUser;
    }

    public async Task<User?> FindUserByClaimId(string claimId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        return user;
    }
}

public interface IUserService
{
    Task<User> CreateUser(UserCreate user);
    Task<User?> FindUserByClaimId(string claimId);
}