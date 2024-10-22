using System.ComponentModel.DataAnnotations;

namespace eUNI_API.Models.Dto;

public class LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}