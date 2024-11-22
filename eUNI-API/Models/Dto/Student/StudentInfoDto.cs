using eUNI_API.Models.Dto.FieldOfStudy;

namespace eUNI_API.Models.Dto.Student;

public class StudentInfoDto
{  
    public int Id { get; set; }
    public string? AlbumNumber { get; set; }
    public bool IsRepresentative { get; set; }
    public IEnumerable<FieldOfStudyInfoDto>? FieldsOfStudyInfo { get; set; }
}