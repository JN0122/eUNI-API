using eUNI_API.Models.Dto.Group;

namespace eUNI_API.Models.Dto.Student;

public class StudentFieldOfStudyDto
{
    public int FieldOfStudyLogId { get; set; }
    public byte Semester { get; set; }
    public string Name { get; set; }
    public int StudiesCycle { get; set; }
    public bool IsRepresentative { get; set; }
    public bool IsFullTime { get; set; }
    public IEnumerable<GroupDto>? Groups { get; set; }
}