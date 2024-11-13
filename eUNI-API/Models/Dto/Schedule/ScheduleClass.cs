namespace eUNI_API.Models.Dto.Schedule;

public class ScheduleClass
{
    public int Hours { get; set; }
    public string Name { get; set; }
    public string Room { get; set; }
    public int Type { get; set; }
    public ClassAssignment? Assignment { get; set; }
}