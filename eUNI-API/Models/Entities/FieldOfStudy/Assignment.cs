using System.ComponentModel.DataAnnotations;

namespace eUNI_API.Models.Entities.FieldOfStudy;

public class Assignment
{
    [Key]
    public int Id { get; set; }
    
    public int ClassId { get; set; }
    
    [MaxLength(100)]
    public string Name { get; set; }
    
    public DateOnly DeadlineDate { get; set; }
    
    public Class Class { get; set; }
}