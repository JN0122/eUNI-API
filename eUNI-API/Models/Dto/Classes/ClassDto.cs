using System.ComponentModel.DataAnnotations;
using eUNI_API.Enums;

namespace eUNI_API.Models.Dto.Classes;

public class ClassDto
{
    public int Id { get; set; }
    
    public int FieldOfStudyLogId { get; set; }
    
    public string Name { get; set; }
    
    public string Room { get; set; }
    
    public bool? IsOddWeek { get; set; }
    
    public WeekDay? WeekDay { get; set; }

    public string GroupName { get; set; }
    
    public string StartHour { get; set; }
    
    public string EndHour { get; set; }
}