using System.ComponentModel.DataAnnotations;

namespace eUNI_API.Models.Dto.Auth;

public class RegisterRequest
{
    [Required, MaxLength(50)] 
    public string FirstName { get; set; } = "";
    
    [Required, MaxLength(50)]
    public string LastName { get; set; } = "";
    
    [Required, MaxLength(60), EmailAddress]
    public string Email { get; set; } = "";
    
    [Required, MaxLength(60)]
    public string Password { get; set; } = "";
    
    [Required, MaxLength(60)]
    public string RepeatPassword { get; set; } = "";

    [Required] 
    public bool AgreedToTerms { get; set; } = false;
}