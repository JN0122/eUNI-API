using System.ComponentModel.DataAnnotations;

namespace eUNI_API.Models.Dto.Classes;

public class CreateAssignmentRequestDto
{
    [Required]
    public int ClassId { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public DateOnly DeadlineDate { get; set; }
}