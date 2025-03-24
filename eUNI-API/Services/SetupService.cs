using eUNI_API.Enums;
using eUNI_API.Exception;
using eUNI_API.Repositories.Interfaces;
using eUNI_API.Services.Interfaces;

namespace eUNI_API.Services;

public class SetupService(IUserRepository userRepository): ISetupService
{
    private readonly IUserRepository _userRepository = userRepository;
    
    public async Task ResetRootAccount(string newPassword)
    {
        _userRepository.RemoveUserByEmail("root@euni.com");
        await _userRepository.CreateUser(
            "Jan", 
            "Kowalski", 
            "root@euni.com", 
            newPassword, 
            (int)UserRole.Admin
        );
        
    }
}