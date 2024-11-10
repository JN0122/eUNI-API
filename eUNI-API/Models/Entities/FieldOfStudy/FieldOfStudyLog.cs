using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eUNI_API.Models.Entities.OrganizationInfo;
using eUNI_API.Models.Entities.Student;

namespace eUNI_API.Models.Entities.FieldOfStudy;

public class FieldOfStudyLog
{
    [Key]
    public int Id { get; set; }

    public int FieldOfStudyId { get; set; }

    public int OrganizationsOfTheYearId { get; set; }

    public int Semester { get; set; }
    
    [ForeignKey("FieldOfStudyId")]
    public FieldOfStudy FieldOfStudy { get; set; }
    
    [ForeignKey("OrganizationsOfTheYearId")]
    public OrganizationOfTheYear OrganizationsOfTheYear { get; set; }
    
    public IEnumerable<StudentFieldsOfStudyLog> StudentsFieldsOfStudyLog { get; set; }
    public IEnumerable<Class> Classes { get; set; }
}