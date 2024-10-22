namespace eUNI_API.Models.Dto.Auth;

public class UserCreate
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Salt { get; set; }
}