using System.ComponentModel.DataAnnotations;

namespace eUNI_API.Models.Dto;

public class UpdateUserRequestDto
{
    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }
    
    [EmailAddress]
    public string? Email { get; set; }
    
    public string? NewPassword { get; set; }
}