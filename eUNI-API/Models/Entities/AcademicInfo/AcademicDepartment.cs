using eUNI_API.Models.Entities.UserInfo;

namespace eUNI_API.Models.Entities.AcademicInfo;

public class AcademicDepartment
{
    public int Id { get; set; }
    public string Abbr { get; set; }

    public ICollection<EmploymentUnit> EmploymentUnits { get; set; }
}