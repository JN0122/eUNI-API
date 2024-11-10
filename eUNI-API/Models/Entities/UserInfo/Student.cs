using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eUNI_API.Models.Entities.Auth;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Models.Entities.UserInfo;

[Index(nameof(AlbumNumber), IsUnique = true)]
public class Student
{
    [Key]
    public int Id { get; set; }
    
    public Guid UserId { get; set; }
    
    [MaxLength(20)]
    public string AlbumNumber { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }
}