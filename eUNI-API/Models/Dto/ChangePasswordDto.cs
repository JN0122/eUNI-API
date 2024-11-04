using System.ComponentModel.DataAnnotations;

namespace eUNI_API.Models.Dto;

public class ChangePasswordDto
{
    public string OldPassword { get; set; }
    
    public string NewPassword { get; set; }
}