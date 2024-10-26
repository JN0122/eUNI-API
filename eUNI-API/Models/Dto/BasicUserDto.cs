using eUNI_API.Models.Entities.User;

namespace eUNI_API.Models.Dto;

public class BasicUserDto
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
}