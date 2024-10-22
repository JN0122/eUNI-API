using System.ComponentModel.DataAnnotations;
using eUNI_API.Enums;
using eUNI_API.Models.Entities.User;

namespace eUNI_API.Models.Dto;

public class RegistrationRequest
{
    [Required]
    public string Firstname { get; set; }

    [Required]
    public string Lastname { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
    
    [Required]
    public string ConfirmPassword { get; set; }
}