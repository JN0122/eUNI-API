using System.ComponentModel.DataAnnotations;

namespace eUNI_API.Models.Dto;

public class ChangeEmailDto
{
    [EmailAddress]
    public string Email { get; set; }
}