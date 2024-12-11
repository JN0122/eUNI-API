using System.ComponentModel.DataAnnotations;

namespace eUNI_API.Models.Dto.FieldOfStudy;

public class CreateFieldOfStudyRequest
{
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Abbr { get; set; }
    
    [Required]
    public byte StudiesCycle { get; set; }
    
    [Required]
    public byte SemesterCount { get; set; }
        
    [Required]
    public bool FullTime { get; set; }
}