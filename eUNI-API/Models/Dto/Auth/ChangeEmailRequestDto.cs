using System.ComponentModel.DataAnnotations;

namespace eUNI_API.Models.Dto.Auth;

public class ChangeEmailRequestDto
{
    [Required, EmailAddress]
    public string Email { get; set; }
}