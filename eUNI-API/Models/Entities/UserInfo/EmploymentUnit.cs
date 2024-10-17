using eUNI_API.Models.Entities.AcademicInfo;

namespace eUNI_API.Models.Entities.UserInfo;

public class EmploymentUnit
{
    public int Id { get; set; }
    public string Abbr { get; set; }
    public int AcademicDepartmentId { get; set; }

    public ICollection<Lecturer> Lecturers { get; set; }
    public AcademicDepartment AcademicDepartment { get; set; }
}