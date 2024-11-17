using System.ComponentModel.DataAnnotations;

namespace eUNI_API.Models.Dto.Schedule;

public class ScheduleInfoDto
{
    [Required]
    public int WeekNumber { get; set; }
    
    [Required]
    public int Year { get; set; }
    
    [Required]
    public int fieldOfStudyLogId { get; set; }
    
    [Required]
    public List<int> GroupIds { get; set; }
}