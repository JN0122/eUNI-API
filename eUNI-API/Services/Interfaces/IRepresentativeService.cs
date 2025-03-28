using eUNI_API.Models.Dto;
using eUNI_API.Models.Dto.Classes;
using eUNI_API.Models.Dto.FieldOfStudy;
using eUNI_API.Models.Dto.Group;
using eUNI_API.Models.Dto.User;
using eUNI_API.Models.Entities.Auth;

namespace eUNI_API.Services.Interfaces;

public interface IRepresentativeService
{
    public Task<IEnumerable<FieldOfStudyInfoDto>?> FieldOfStudyLogsToEdit(Guid userId);
    public UserInfoDto GetUserInfoDto(User user);
    public IEnumerable<UserInfoDto> GetUsersInfoDto(IEnumerable<User> users);
    public Task<IEnumerable<ClassDto>> GetClasses(int fieldOfStudyLogId);
    public Task CreateClass(CreateClassRequestDto classRequestDto);
    public Task UpdateClass(int classId, UpdateClassRequestDto updateClassRequestDto);
    public Task DeleteClass(int classId);
    public IEnumerable<GroupDto> GetAllGroups(int fieldOfStudyLogId);
}