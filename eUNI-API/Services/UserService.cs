using System.Security.Claims;
using eUNI_API.Data;
using eUNI_API.Enums;
using eUNI_API.Helpers;
using eUNI_API.Models.Dto;
using eUNI_API.Models.Entities.Auth;
using eUNI_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Services;

public class UserService(AppDbContext context): IUserService
{
    private readonly AppDbContext _context = context;

    public async Task<User> CreateUser(CreateUserDto createUserDto)
    {
        var studentRole = _context.Roles.FirstOrDefault(role => role.Id == (int)UserRole.Student);

        if (studentRole == null)
            throw new ArgumentException("Student role doesn't exist");

        var newUser = new User
        {
            Email = createUserDto.Email,
            FirstName = createUserDto.Firstname,
            LastName = createUserDto.Lastname,
            PasswordHash = createUserDto.PasswordHash,
            Salt = createUserDto.Salt,
            Role = studentRole,
        };
        
        await _context.Users.AddAsync(newUser);
        _context.SaveChanges();
        return newUser;
    }

    public async Task<User> FindUserByClaim(IEnumerable<Claim> claims)
    {
        var userIdClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        
        if(userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
            throw new ArgumentException("Invalid user ID claim present in token.");

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            throw new ArgumentException("Invalid user ID");

        return user;
    }

    public void ChangePassword(User user, string newPassword)
    {
        var newSalt = PasswordHasher.GenerateSalt();
        
        user.Salt = newSalt;
        user.PasswordHash = PasswordHasher.HashPassword(newPassword, newSalt);
        
        _context.Users.Update(user);
        _context.SaveChanges();
    }

    public void ChangeEmail(User user, string newEmail)
    {
        user.Email = newEmail;
        
        _context.Users.Update(user);
        _context.SaveChanges();
    }
}
