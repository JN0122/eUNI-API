using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eUNI_API.Models.Entities.FieldOfStudy;

namespace eUNI_API.Models.Entities.Student;

public class StudentFieldsOfStudyLog
{
    [Key]
    public int Id { get; set; }

    public int FieldsOfStudyLogId { get; set; }
    
    public int StudentId { get; set; }

    public bool IsRepresentative { get; set; } = false;
    
    [ForeignKey("FieldsOfStudyLogId")]
    public FieldOfStudyLog FieldsOfStudyLog { get; set; }
    
    [ForeignKey("StudentId")]
    public Student Student { get; set; }
    
    public IEnumerable<StudentGroup> StudentGroups { get; set; }
}