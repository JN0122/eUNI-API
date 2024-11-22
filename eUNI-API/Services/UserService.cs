using System.Security.Claims;
using eUNI_API.Data;
using eUNI_API.Helpers;
using eUNI_API.Models.Dto;
using eUNI_API.Models.Entities.Auth;
using eUNI_API.Repositories.Interfaces;
using eUNI_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Services;

public class UserService(AppDbContext context, IStudentRepository studentRepository, IOrganizationRepository organizationRepository, IAuthRepository authRepository): IUserService
{
    private readonly AppDbContext _context = context;
    private readonly IStudentRepository _studentRepository = studentRepository;
    private readonly IOrganizationRepository _organizationRepository = organizationRepository;
    private readonly IAuthRepository _authRepository = authRepository;

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

    public UserInfoDto GetUserInfo(User user)
    {
        return new UserInfoDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            RoleId = user.RoleId
        };
    }

    public IEnumerable<UserInfoDto> GetUsersInfo(IEnumerable<User> users)
    {
        return users.Select(GetUserInfo);
    }
}
