using eUNI_API.Models.Dto;
using eUNI_API.Models.Entities.Auth;

namespace eUNI_API.Helpers;

public static class ConvertDtos
{
    public static BasicUserDto ToBasicUserDto(User user)
    {
        return new BasicUserDto
        {
            Id = user.Id,
            Firstname = user.FirstName,
            Lastname = user.LastName,
            Email = user.Email,
            Role = user.RoleId
        };
    }

    public static UserInfoDto ToUserInfoDto(User user)
    {
        return new UserInfoDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };
    }
    
    public static IEnumerable<UserInfoDto> ToUserInfoDto(IEnumerable<User> users)
    {
        var usersInfo = new List<UserInfoDto>();
        usersInfo.AddRange(users.Select(ToUserInfoDto));
        return usersInfo;
    }
}