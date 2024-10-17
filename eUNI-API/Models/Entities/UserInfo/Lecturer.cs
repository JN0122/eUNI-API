namespace eUNI_API.Models.Entities.UserInfo;

public class Lecturer
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public int EmploymentUnitId { get; set; }

    public User.User User { get; set; }
    public EmploymentUnit EmploymentUnit { get; set; }
}