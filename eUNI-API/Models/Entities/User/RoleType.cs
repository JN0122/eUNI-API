namespace eUNI_API.Models.Entities.User;

public class RoleType
{
    public int Id { get; set; }
    public string Name { get; set; }

    //Navigation properties
    public ICollection<Role> Roles { get; set; }
}