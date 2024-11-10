using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eUNI_API.Models.Entities.UserInfo;

public class StudentGroup
{
    [Key]
    public int Id { get; set; }
    
    public int StudentsFieldsOfStudyLogId { get; set; }

    public int GroupId { get; set; }
    
    public bool IsRepresentative { get; set; }
    
    [ForeignKey("StudentsFieldsOfStudyLogId")]
    public StudentFieldsOfStudyLog StudentsFieldsOfStudyLog { get; set; }
    
    [ForeignKey("GroupId")]
    public Group Group { get; set; }
}