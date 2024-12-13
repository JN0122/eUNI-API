using eUNI_API.Configuration;
using eUNI_API.Enums;
using eUNI_API.Exception;
using eUNI_API.Helpers;
using eUNI_API.Models.Dto.Auth;
using eUNI_API.Models.Entities.Auth;
using eUNI_API.Repositories.Interfaces;
using eUNI_API.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace eUNI_API.Services;

public class AuthService(IOptions<JwtSettings> jwtSettings, IAuthRepository authRepository, 
        IUserRepository userRepository): IAuthService
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;
    private readonly IAuthRepository _authRepository = authRepository;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<User> Register(RegisterRequest registerRequest)
    {
        if(registerRequest.Password != registerRequest.RepeatPassword) 
            throw new HttpBadRequestHttpException("Passwords don't match");
        
        if(registerRequest.AgreedToTerms == false) 
            throw new HttpBadRequestHttpException("Please agree to terms!");

        return await _userRepository.CreateUser(
            registerRequest.FirstName, 
            registerRequest.LastName, 
            registerRequest.Email,
            registerRequest.Password,
            (int)UserRole.Student);
    }

    public async Task<User> Login(LoginDto loginDto)
    {
        var user = await _authRepository.GetUserWithRole(loginDto.Email);

        if (user == null || user.IsDeleted)
            throw new HttpUnauthorizedHttpException();
    
        var isValidPassword = PasswordHasher.VerifyHashedPassword(loginDto.Password, user.Salt, user.PasswordHash);

        if (!isValidPassword)
            throw new HttpUnauthorizedHttpException();
        return user;
    }

    public void AddRefreshToken(IResponseCookies cookies, string refreshToken)
    {
        cookies.Append("refresh-token", refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
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