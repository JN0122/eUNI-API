using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eUNI_API.Models.Entities.Student;

namespace eUNI_API.Models.Entities.Auth;
public class User
{
    [Key]
    public Guid Id { get; set; }
    
    [MaxLength(40)]
    public string FirstName { get; set; }
    
    [MaxLength(70)]
    public string LastName { get; set; }
    
    [EmailAddress]
    public string Email { get; set; }
    
    [Base64String]
    public string PasswordHash { get; set; }
    
    [Base64String]
    public string Salt { get; set; }
    
    public bool IsDeleted { get; set; } = false;
    
    public int RoleId { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [ForeignKey("RoleId")]
    public Role Role { get; set; }
    
    public IEnumerable<StudentFieldsOfStudyLog>? StudentFieldsOfStudyLogs { get; set; }
    public ICollection<PasswordResetLog>? PasswordResetLogs { get; set; }
    public ICollection<RefreshToken>? RefreshTokens { get; set; }
}