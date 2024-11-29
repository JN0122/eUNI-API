using eUNI_API.Models.Dto.FieldOfStudy;

namespace eUNI_API.Models.Dto.Student;

public class StudentInfoDto
{  
    public Guid Id { get; set; }
    public IEnumerable<StudentFieldOfStudyDto>? FieldsOfStudyInfo { get; set; }
}