namespace eUNI_API.Models.Dto.Classes;

public class AssignmentDto
{
    public int Id { get; set; }
    public string AssignmentName { get; set; }
    public DateOnly DeadlineDate { get; set; }
    public int ClassId { get; set; }
    public string ClassName { get; set; }
}