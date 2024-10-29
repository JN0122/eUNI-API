using eUNI_API.Models.Dto;
using eUNI_API.Models.Entities.User;

namespace eUNI_API.Helpers;

public static class ConvertDtos
{
    public static BasicUserDto ToBasicUserDto(User user)
    {
        return new BasicUserDto
        {
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            Email = user.Email
        };
    }
}