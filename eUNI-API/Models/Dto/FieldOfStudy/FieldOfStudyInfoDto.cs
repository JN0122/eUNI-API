namespace eUNI_API.Models.Dto.FieldOfStudy;

public class FieldOfStudyInfoDto
{
    public int FieldOfStudyLogId { get; set; }
    public byte Semester { get; set; }
    public string Name { get; set; }
    public int StudiesCycle { get; set; }
    public bool IsFullTime { get; set; }
    public int YearId { get; set; }
    public bool FirstHalfOfYear { get; set; }
    public string YearName { get; set; }
}