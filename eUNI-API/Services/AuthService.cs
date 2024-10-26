using eUNI_API.Models.Dto;
using eUNI_API.Models.Entities.User;
using eUNI_API.Services.Interfaces;

namespace eUNI_API.Services;

public class AuthService(IUserService userService): IAuthService
{
    private readonly IUserService _userService = userService;
    
    public async Task<User> Register(RegistrationDto registrationDto)
    {
        throw new NotImplementedException();
    }

    public async Task<User>? Login(LoginDto loginDto)
    {
        throw new NotImplementedException();
    }
}