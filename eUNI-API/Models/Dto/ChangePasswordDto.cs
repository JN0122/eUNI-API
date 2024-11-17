using System.ComponentModel.DataAnnotations;

namespace eUNI_API.Models.Dto;

public class ChangePasswordDto
{
    [Required]
    public string OldPassword { get; set; }
    
    [Required]
    public string NewPassword { get; set; }
}