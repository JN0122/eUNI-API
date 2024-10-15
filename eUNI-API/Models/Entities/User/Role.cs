namespace eUNI_API.Models.Entities.User;

public class Role
{
    public int Id { get; set; }

    public int RoleTypeId { get; set; }

    //Navigation properties
    public ICollection<FieldOfStudy> FieldsOfStudy { get; set; }
    public EmploymentUnit EmploymentUnit { get; set; }
    public RoleType RoleType { get; set; }
    public User User { get; set; }
}