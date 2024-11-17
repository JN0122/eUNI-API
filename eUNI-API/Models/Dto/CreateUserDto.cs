using System.ComponentModel.DataAnnotations;

namespace eUNI_API.Models.Dto;

public class CreateUserDto
{
    public string Firstname { get; set; }

    public string Lastname { get; set; }

    [EmailAddress]
    public string Email { get; set; }
    
    public int RoleId { get; set; }

    public string Password { get; set; }
}