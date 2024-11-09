using eUNI_API.Models.Entities.Auth;

namespace eUNI_API.Models.Entities.UserInfo;

public class Student
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public string AlbumNumber { get; set; }

    public User User { get; set; }
}