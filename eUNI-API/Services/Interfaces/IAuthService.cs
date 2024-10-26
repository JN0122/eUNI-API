using eUNI_API.Models.Dto;
using eUNI_API.Models.Entities.User;
using Microsoft.AspNetCore.Identity.Data;

namespace eUNI_API.Services.Interfaces;

public interface IAuthService
{
    public Task<User> Register(RegistrationDto registrationDto);
    public Task<User> Login(LoginDto loginDto);
    public void AddRefreshToken(IResponseCookies cookies, string refreshToken);
    public void RemoveRefreshToken(IResponseCookies cookies);
}