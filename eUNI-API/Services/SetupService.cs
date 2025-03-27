using eUNI_API.Enums;
using eUNI_API.Repositories.Interfaces;
using eUNI_API.Services.Interfaces;

namespace eUNI_API.Services;

public class SetupService(IUserRepository userRepository, IOrganizationRepository organizationRepository, 
    IFieldOfStudyRepository fieldOfStudyRepository, IStudentRepository studentRepository, 
    IClassesRepository classesRepository, IAuthRepository authRepository): ISetupService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IOrganizationRepository _organizationRepository = organizationRepository;
    private readonly IFieldOfStudyRepository _fieldOfStudyRepository = fieldOfStudyRepository;
    private readonly IStudentRepository _studentRepository = studentRepository;
    private readonly IClassesRepository _classesRepository = classesRepository;
    private readonly IAuthRepository _authRepository = authRepository;
    
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

    public async void ResetDb()
    {
        await _authRepository.RevokeAllTokens();
        await _classesRepository.DeleteAllClasses();
        await _fieldOfStudyRepository.DeleteAllFieldsOfStudy();
        await _studentRepository.DeleteAllStudentGroups();
        
        await _fieldOfStudyRepository.DeleteAllFieldOfStudyLogs();
        await _studentRepository.DeleteAllStudentLogs();
        _userRepository.DeleteAllUsers();
    }
}