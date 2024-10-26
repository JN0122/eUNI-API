using eUNI_API.Data;
using eUNI_API.Enums;
using eUNI_API.Models.Dto;
using eUNI_API.Models.Entities.User;
using eUNI_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Services;

public class UserService(AppDbContext context): IUserService
{
    private readonly AppDbContext _context = context;

    public async Task<User> CreateUser(CreateUser createUser)
    {
        var studentRole = _context.Roles.FirstOrDefault(role => role.Id == (int)UserRole.Student);

        if (studentRole == null)
            throw new Exception("Student role doesn't exist");

        var newUser = new User
        {
            Email = createUser.Email,
            Firstname = createUser.Firstname,
            Lastname = createUser.Lastname,
            PasswordHash = createUser.PasswordHash,
            Salt = createUser.Salt,
            Role = studentRole,
        };
        
        await _context.Users.AddAsync(newUser);
        _context.SaveChanges();
        return newUser;
    }

    public async Task<User?> FindUserByClaimId(string claimId)
    {
        if (!Guid.TryParse(claimId, out var userId)){
            return null;
        }

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId);

        return user;
    }
}
