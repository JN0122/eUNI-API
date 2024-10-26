using System.ComponentModel.DataAnnotations;

namespace eUNI_API.Models.Dto;

public class RegistrationDto
{
    [Required]
    public string? Firstname { get; set; }

    [Required]
    public string? Lastname { get; set; }

    [Required, EmailAddress]
    public string? Email { get; set; }

    [Required]
    public string? Password { get; set; }
    
    [Required]
    public string? ConfirmPassword { get; set; }
}