using eUNI_API.Models.Dto;
using eUNI_API.Models.Dto.FieldOfStudy;
using eUNI_API.Models.Entities.Auth;
using eUNI_API.Models.Entities.FieldOfStudy;

namespace eUNI_API.Helpers;

public static class ConvertDtos
{
    public static UserInfoDto ToUserInfoDto(User user)
    {
        return new UserInfoDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            RoleId = user.RoleId,
        };
    }
    
    public static IEnumerable<UserInfoDto> ToUserInfoDto(IEnumerable<User> users)
    {
        var usersInfo = new List<UserInfoDto>();
        usersInfo.AddRange(users.Select(ToUserInfoDto));
        return usersInfo;
    }

    public static FieldOfStudyInfoDto ToFieldOfStudyInfoDto(FieldOfStudyLog fieldOfStudyLog)
    {
        return new FieldOfStudyInfoDto
        {
            FieldOfStudyLogId = fieldOfStudyLog.Id,
            Name = fieldOfStudyLog.FieldOfStudy.Name,
            Semester = fieldOfStudyLog.Semester,
            StudiesCycle = fieldOfStudyLog.FieldOfStudy.StudiesCycle
        };
    }
}