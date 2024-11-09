using System.Data;
using eUNI_API.Configuration;
using eUNI_API.Data;
using eUNI_API.Helpers;
using eUNI_API.Models.Dto;
using eUNI_API.Models.Entities.Auth;
using eUNI_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace eUNI_API.Services;

public class AuthService(IUserService userService, AppDbContext context, IOptions<JwtSettings> jwtSettings): IAuthService
{
    private readonly AppDbContext _context = context;
    private readonly IUserService _userService = userService;
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;
    
    public async Task<User> Register(RegistrationDto registrationDto)
    {
        if(!registrationDto.Password.Equals(registrationDto.ConfirmPassword))
            throw new ArgumentException("Passwords do not match!");

        var salt = PasswordHasher.GenerateSalt();
        var userCreate = new CreateUserDto
        {
            Firstname = registrationDto.Firstname,
            Lastname = registrationDto.Lastname,
            Email = registrationDto.Email,
            PasswordHash = PasswordHasher.HashPassword(registrationDto.Password, salt),
            Salt = salt
        };

        return await _userService.CreateUser(userCreate);
    }

    public async Task<User> Login(LoginDto loginDto)
    {
        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

        if (user == null || user.IsDeleted)
            throw new UnauthorizedAccessException();
    
        var isValidPassword = PasswordHasher.VerifyHashedPassword(loginDto.Password, user.Salt, user.PasswordHash);

        if (!isValidPassword)
            throw new UnauthorizedAccessException();
        return user;
    }

    public void AddRefreshToken(IResponseCookies cookies, string refreshToken)
    {
        cookies.Append("refresh-token", refreshToken, new CookieOptions
        {
            HttpOnly = true,
            //Secure = true, //https
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays)
        });
    }

    public void RemoveRefreshToken(IResponseCookies cookies)
    {
        cookies.Delete("refresh-token");
    }

    public string? GetRefreshToken(IRequestCookieCollection cookies)
    {
        cookies.TryGetValue("refresh-token", out string? refreshToken);
        return refreshToken;
    }
}