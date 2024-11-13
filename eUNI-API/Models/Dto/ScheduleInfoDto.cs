namespace eUNI_API.Models.Dto;

public class ScheduleInfoDto
{
    public int WeekNumber { get; set; }
    public int Year { get; set; }
    public int FieldOfStudyLogsId { get; set; }
    public List<int> GroupIds { get; set; }
}