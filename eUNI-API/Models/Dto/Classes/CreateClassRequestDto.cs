using System.ComponentModel.DataAnnotations;

namespace eUNI_API.Models.Dto.Classes;

public class CreateClassRequestDto
{
    [Required]
    public int FieldOfStudyId { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Room { get; set; }
    
    [Required]
    public bool IsOddWeek { get; set; }
    
    [Required]
    public byte WeekDay { get; set; }
    
    [Required]
    public int GroupId { get; set; }
    
    [Required]
    public int StartHourId { get; set; }
    
    [Required]
    public int EndHourId { get; set; }
}