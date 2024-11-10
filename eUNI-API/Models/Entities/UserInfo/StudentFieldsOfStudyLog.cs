using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eUNI_API.Models.Entities.FieldOfStudy;

namespace eUNI_API.Models.Entities.UserInfo;

public class StudentFieldsOfStudyLog
{
    [Key]
    public int Id { get; set; }

    public int FieldsOfStudyLogId { get; set; }
    
    public int StudentId { get; set; }
    
    [ForeignKey("FieldsOfStudyLogId")]
    public FieldOfStudyLog FieldsOfStudyLog { get; set; }
    
    [ForeignKey("StudentId")]
    public Student Student { get; set; }
    
    public IEnumerable<StudentGroup> StudentGroups { get; set; }
}