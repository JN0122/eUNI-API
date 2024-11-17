using System.ComponentModel.DataAnnotations;

namespace eUNI_API.Models.Dto;

public class ChangeEmailRequestDto
{
    [Required, EmailAddress]
    public string Email { get; set; }
}