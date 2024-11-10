using System.ComponentModel.DataAnnotations;

namespace eUNI_API.Models.Entities.Auth;

public class Role
{
    [Key]
    public int Id { get; set; }
    
    [MaxLength(30)]
    public string Name { get; set; }
    
    public ICollection<User> Users { get; set; }
}