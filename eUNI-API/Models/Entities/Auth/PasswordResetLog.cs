using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Models.Entities.Auth;

[Index(nameof(Token), IsUnique = true)]
public class PasswordResetLog
{
    [Key]
    public int Id { get; set; }
    
    public Guid UserId { get; set; }
    
    public string Token { get; set; }
    
    public DateTime UsedAt { get; set; }
    
    public DateTime ExpiresAt { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }
}