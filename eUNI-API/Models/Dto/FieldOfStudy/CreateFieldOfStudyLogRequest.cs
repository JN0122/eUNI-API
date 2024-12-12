namespace eUNI_API.Models.Dto.FieldOfStudy;

public class CreateFieldOfStudyLogRequest
{
    public int FieldOfStudyId { get; set; }
    public int OrganizationId { get; set; }
    public byte CurrentSemester { get; set; }
}