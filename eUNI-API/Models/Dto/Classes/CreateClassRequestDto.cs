using System.ComponentModel.DataAnnotations;
using eUNI_API.Enums;

namespace eUNI_API.Models.Dto.Classes;

public class CreateClassRequestDto
{
    [Required]
    public int FieldOfStudyLogId { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Room { get; set; }
    
    public bool? IsOddWeek { get; set; }
    
    [Required]
    public WeekDay WeekDay { get; set; }
    
    [Required]
    public int GroupId { get; set; }
    
    [Required]
    public int StartHourId { get; set; }
    
    [Required]
    public int EndHourId { get; set; }
}