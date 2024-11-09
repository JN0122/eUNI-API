using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace eUNI_API.Models.Entities.Auth;

public class RefreshToken
{
    [Key]
    public int Id { get; set; }
    
    public Guid UserId { get; set; }
    
    [Base64String, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Token { get; set; }
    
    public DateTime Expires { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }
}