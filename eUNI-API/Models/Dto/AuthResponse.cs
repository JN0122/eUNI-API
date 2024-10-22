using eUNI_API.Models.Entities.User;

namespace eUNI_API.Models.Dto;

public class AuthResponse
{
    public Guid UserId { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string Token { get; set; }
}