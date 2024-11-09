namespace eUNI_API.Models.Entities.Auth;

public class PasswordResetLog
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public string Token { get; set; }
    public DateTime Datetime { get; set; }

    public User User { get; set; }
}