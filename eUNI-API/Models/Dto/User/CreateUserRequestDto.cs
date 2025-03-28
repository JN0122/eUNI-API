using System.ComponentModel.DataAnnotations;

namespace eUNI_API.Models.Dto.User;

public class CreateUserRequestDto
{
    [Required]
    public string Firstname { get; set; }

    [Required]
    public string Lastname { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public int RoleId { get; set; }

    [Required]
    public string Password { get; set; }

    public List<int> RepresentativeFieldsOfStudyLogIds { get; set; } = [];
}