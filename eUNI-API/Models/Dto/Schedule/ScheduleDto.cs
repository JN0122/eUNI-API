namespace eUNI_API.Models.Dto.Schedule;

public class ScheduleDto
{
    public string Date { get; set; }
    public List<ScheduleWeekDays> Schedule { get; set; } = [];
}