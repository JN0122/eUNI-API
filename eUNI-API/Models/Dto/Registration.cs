using System.ComponentModel.DataAnnotations;

namespace eUNI_API.Models.Dto;

public class Registration
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