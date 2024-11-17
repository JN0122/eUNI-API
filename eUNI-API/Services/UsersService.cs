using eUNI_API.Data;
using eUNI_API.Helpers;
using eUNI_API.Models.Dto;
using eUNI_API.Models.Entities.Auth;
using eUNI_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Services;

public class UsersService(AppDbContext context, IUserService userService): IUsersService
{
    private readonly AppDbContext _context = context;
    private readonly IUserService _userService = userService;
    
    private async Task<bool> IsValidRole(int roleId)
    {
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
        return role != null;
    }
    
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
    
    public async Task CreateUser(CreateUserRequestDto createUserRequestDto)
    {
        if(!await IsValidRole(createUserRequestDto.RoleId))
            throw new ArgumentException("Invalid role");
        
        var salt = PasswordHasher.GenerateSalt();
        
        var newUser = new User
        {
            Email = createUserRequestDto.Email,
            FirstName = createUserRequestDto.Firstname,
            LastName = createUserRequestDto.Lastname,
            PasswordHash = PasswordHasher.HashPassword(createUserRequestDto.Password, salt),
            RoleId = createUserRequestDto.RoleId,
            Salt = salt,
        };
        
        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();
    }

    public async Task<User> GetUserById(Guid id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null || user.IsDeleted)
            throw new BadHttpRequestException("Could not find user");
        
        return user;
    }

    public async Task UpdateUser(User user, UpdateUserRequestDto updateUserRequestDto)
    {
        if (updateUserRequestDto.FirstName != null)
            user.FirstName = updateUserRequestDto.FirstName;
        
        if (updateUserRequestDto.LastName != null)
            user.LastName = updateUserRequestDto.LastName;
        
        if (updateUserRequestDto.Email != null)
            user.Email = updateUserRequestDto.Email;
        
        if (updateUserRequestDto.RoleId != null)
            user.RoleId = updateUserRequestDto.RoleId.Value;

        if (updateUserRequestDto.NewPassword != null)
            _userService.ChangePassword(user, updateUserRequestDto.NewPassword);
        
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}