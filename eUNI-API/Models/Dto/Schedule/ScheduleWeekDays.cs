namespace eUNI_API.Models.Dto.Schedule;

public class ScheduleWeekDays
{
    public int Id { get; set; }
    public string Hour { get; set; }
    public ScheduleClass? Monday { get; set; }
    public ScheduleClass? Tuesday { get; set; }
    public ScheduleClass? Wednesday { get; set; }
    public ScheduleClass? Thursday { get; set; }
    public ScheduleClass? Friday { get; set; }
    public ScheduleClass? Saturday { get; set; }
    public ScheduleClass? Sunday { get; set; }
}