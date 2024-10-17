namespace eUNI_API.Models.Entities.UserInfo;

public class Student
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public string AlbumNumber { get; set; }

    public User.User User { get; set; }
}