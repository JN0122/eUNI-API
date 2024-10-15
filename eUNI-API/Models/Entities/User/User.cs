using System.ComponentModel.DataAnnotations.Schema;

namespace eUNI_API.Models.Entities.User;
public class User
{
    public Guid Id { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public byte[] Salt { get; set; }

    public int RoleId { get; set; }
    
    //Navigation properties
    public Role Role { get; set; }
}