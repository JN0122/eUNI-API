using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Models.Entities.Auth;

[Index(nameof(Token), IsUnique = true)]
public class RefreshToken
{
    [Key]
    public int Id { get; set; }
    
    public Guid UserId { get; set; }
    
    public string Token { get; set; }
    
    public DateTime Expires { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }
}