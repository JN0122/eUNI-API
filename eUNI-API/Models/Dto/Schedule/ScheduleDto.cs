namespace eUNI_API.Models.Dto.Schedule;

public class ScheduleDto
{
    public string Date { get; set; }
    public bool CanFetchPreviousWeek { get; set; }
    public bool CanFetchNextWeek { get; set; }
    public List<ScheduleWeekDays> Schedule { get; set; } = [];
}