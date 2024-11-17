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
    
    public async Task CreateUser(CreateUserDto createUserDto)
    {
        var salt = PasswordHasher.GenerateSalt();
        
        var newUser = new User
        {
            Email = createUserDto.Email,
            FirstName = createUserDto.Firstname,
            LastName = createUserDto.Lastname,
            PasswordHash = PasswordHasher.HashPassword(createUserDto.Password, salt),
            RoleId = createUserDto.RoleId,
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

    public async Task UpdateUser(User user, UpdateUserDto updateUserDto)
    {
        if (updateUserDto.FirstName != null)
            user.FirstName = updateUserDto.FirstName;
        
        if (updateUserDto.LastName != null)
            user.LastName = updateUserDto.LastName;
        
        if (updateUserDto.Email != null)
            user.Email = updateUserDto.Email;

        if (updateUserDto.NewPassword != null)
            _userService.ChangePassword(user, updateUserDto.NewPassword);
        
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}