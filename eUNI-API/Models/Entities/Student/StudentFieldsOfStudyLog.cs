using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eUNI_API.Models.Entities.Auth;
using eUNI_API.Models.Entities.FieldOfStudy;

namespace eUNI_API.Models.Entities.Student;

public class StudentFieldsOfStudyLog
{
    [Key]
    public int Id { get; set; }

    public int FieldsOfStudyLogId { get; set; }
    
    public Guid UserId { get; set; }

    public bool IsRepresentative { get; set; }
    
    public bool IsCurrentFieldOfStudy { get; set; }
    
    [ForeignKey("FieldsOfStudyLogId")]
    public FieldOfStudyLog FieldsOfStudyLog { get; set; }
    
    [ForeignKey("UserId")]
    public User User { get; set; }
    
    public IEnumerable<StudentGroup> StudentGroups { get; set; }
}