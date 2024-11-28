using System.ComponentModel.DataAnnotations;

namespace eUNI_API.Models.Dto.Classes;

public class UpdateClassRequestDto
{
    [Required]
    public int FieldOfStudyLogId { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Room { get; set; }
    
    [Required]
    public IEnumerable<DateOnly> Dates { get; set; }
    
    [Required]
    public int GroupId { get; set; }
    
    [Required]
    public int StartHourId { get; set; }
    
    [Required]
    public int EndHourId { get; set; }
}