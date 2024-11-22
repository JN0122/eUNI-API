namespace eUNI_API.Models.Dto.FieldOfStudy;

public class FieldOfStudyGroupDto
{
    public int FieldOfStudyLogId { get; set; }
    public IEnumerable<int>? GroupIds { get; set; }
}