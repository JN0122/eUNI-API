using System.ComponentModel.DataAnnotations.Schema;

namespace eUNI_API.Models.Entities.User;

public class FieldOfStudy
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public int RoleId { get; set; }
    
    //Navigation properties
    public Role Role { get; set; }
}