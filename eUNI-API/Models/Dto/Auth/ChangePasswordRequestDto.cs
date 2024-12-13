using System.ComponentModel.DataAnnotations;

namespace eUNI_API.Models.Dto.Auth;

public class ChangePasswordRequestDto
{
    [Required]
    public string OldPassword { get; set; }
    
    [Required]
    public string NewPassword { get; set; }
}