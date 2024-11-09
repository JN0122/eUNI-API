using System.ComponentModel.DataAnnotations.Schema;
using eUNI_API.Models.Entities.UserInfo;

namespace eUNI_API.Models.Entities.User;
public class User
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Salt { get; set; }
    public bool IsDeleted { get; set; } = false;
    public int RoleId { get; set; }
    public Role Role { get; set; }
    public Student? Student { get; set; }
    public ICollection<PasswordResetLog> PasswordResetLogs { get; set; }
    public ICollection<RefreshToken> RefreshTokens { get; set; }
}