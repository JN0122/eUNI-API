namespace eUNI_API.Models.Dto.FieldOfStudy;

public class CreateFieldOfStudyRequest
{
    public string Name { get; set; }
    public string Abbr { get; set; }
    public int StudiesCycle { get; set; }
    public int SemesterCount { get; set; }
    public bool FullTime { get; set; }
}