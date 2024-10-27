using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace eUNI_API.Models.Entities.User;

public class RefreshToken
{
    [Key]
    public int Id { get; set; }
    
    public Guid UserId { get; set; }
    
    [Base64String]
    public string Token { get; set; }
    
    public DateTime Expires { get; set; }
    
    public bool IsRevoked { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }
}