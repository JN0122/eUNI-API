using System.Security.Claims;
using eUNI_API.Configuration;
using eUNI_API.Data;
using eUNI_API.Enums;
using eUNI_API.Helpers;
using eUNI_API.Models.Dto.Auth;
using eUNI_API.Models.Entities.Auth;
using eUNI_API.Repositories.Interfaces;
using eUNI_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace eUNI_API.Services;

public class AuthService(AppDbContext context, IOptions<JwtSettings> jwtSettings, IAuthRepository authRepository): IAuthService
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;
    private readonly IAuthRepository _authRepository = authRepository;

    public async Task<User> Login(LoginDto loginDto)
    {
        var user = await _authRepository.GetUserWithRole(loginDto.Email);

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