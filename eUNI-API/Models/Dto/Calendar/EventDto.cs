namespace eUNI_API.Models.Dto.Calendar;

public class EventDto
{
    public string ClassName { get; set; }
    public string ClassRoom { get; set; }
    public List<DateOnly> Dates { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
}