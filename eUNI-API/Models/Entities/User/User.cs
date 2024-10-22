using System.ComponentModel.DataAnnotations.Schema;
using eUNI_API.Models.Entities.UserInfo;

namespace eUNI_API.Models.Entities.User;
public class User
{
    public Guid Id { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Salt { get; set; }
    public int RoleId { get; set; }
    public Role Role { get; set; }
    public Lecturer? Lecturer { get; set; }
    public Student? Student { get; set; }
    public ICollection<PasswordResetLog> PasswordResetLog { get; set; }
}