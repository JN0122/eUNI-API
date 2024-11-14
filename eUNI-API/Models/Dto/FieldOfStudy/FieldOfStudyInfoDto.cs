namespace eUNI_API.Models.Dto.FieldOfStudy;

public class FieldOfStudyInfoDto
{
    public int Id { get; set; }
    public byte Semester { get; set; }
    public int FieldOfStudyId { get; set; }
    public string Name { get; set; }
    public int StudiesCycle { get; set; }
}