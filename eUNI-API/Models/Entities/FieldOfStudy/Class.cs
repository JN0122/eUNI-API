using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using eUNI_API.Enums;
using eUNI_API.Models.Entities.OrganizationInfo;
using eUNI_API.Models.Entities.Student;

namespace eUNI_API.Models.Entities.FieldOfStudy;

public class Class
{
    [Key]
    public int Id { get; set; }
    
    public int FieldOfStudyLogId { get; set; }
    
    [MaxLength(50)]
    public string Name { get; set; }
    
    [MaxLength(15)]
    public string Room { get; set; }

    public int GroupId { get; set; }
    
    public int StartHourId { get; set; }
    
    public int EndHourId { get; set; }
    
    [ForeignKey("StartHourId")]
    public Hour StartHour { get; set; }
    
    [ForeignKey("EndHourId")]
    public Hour EndHour { get; set; }
    
    [ForeignKey("FieldOfStudyLogId")]
    public FieldOfStudyLog FieldOfStudyLog { get; set; }
    
    [ForeignKey("GroupId")]
    public Group Group { get; set; }
    public IEnumerable<ClassDate> ClassDates { get; set; }
}