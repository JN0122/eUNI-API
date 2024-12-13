using System.ComponentModel.DataAnnotations;

namespace eUNI_API.Models.Dto.User;

public class UpdateUserRequestDto
{
    [Required]
    public string? FirstName { get; set; }
    
    [Required]
    public string? LastName { get; set; }

    [Required, EmailAddress] 
    public string? Email { get; set; }
    
    [Required]
    public int? RoleId { get; set; }

    [Required]
    public List<int> RepresentativeFieldsOfStudyLogIds { get; set; } = [];
    
    public string? NewPassword { get; set; }
}