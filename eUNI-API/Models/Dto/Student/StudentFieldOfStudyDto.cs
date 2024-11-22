namespace eUNI_API.Models.Dto.Student;

public class StudentFieldOfStudyDto
{
    public int FieldOfStudyLogId { get; set; }
    public byte Semester { get; set; }
    public string Name { get; set; }
    public int StudiesCycle { get; set; }
    public IEnumerable<int>? GroupIds { get; set; }
}