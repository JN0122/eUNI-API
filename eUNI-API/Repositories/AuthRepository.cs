using eUNI_API.Data;
using eUNI_API.Enums;
using eUNI_API.Models.Entities.Auth;
using eUNI_API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Repositories;

public class AuthRepository(AppDbContext context): IAuthRepository
{
    private readonly AppDbContext _context = context;
    
    public async Task<User> GetUserWithRole(string email)
    {
        var user =  await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == email);
        if(user == null) throw new UnauthorizedAccessException("Cannot find user");
        return user;
    }

    public bool IsAdmin(Guid userId)
    {
        return _context.Users.AsNoTracking().FirstOrDefault(u => u.Id == userId)?.RoleId == (int)UserRole.Admin;
    }
}