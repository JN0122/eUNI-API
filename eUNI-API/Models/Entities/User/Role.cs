namespace eUNI_API.Models.Entities.User;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public ICollection<User> Users { get; set; }
}