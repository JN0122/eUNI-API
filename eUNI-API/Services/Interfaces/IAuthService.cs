using eUNI_API.Models.Dto.Auth;
using eUNI_API.Models.Entities.Auth;

namespace eUNI_API.Services.Interfaces;

public interface IAuthService
{
    public Task<User> Register(RegisterRequest registerRequest);
    public Task<User> Login(LoginDto loginDto);
    public void AddRefreshToken(IResponseCookies cookies, string refreshToken);
    public void RemoveRefreshToken(IResponseCookies cookies);
    public string? GetRefreshToken(IRequestCookieCollection cookies);
}