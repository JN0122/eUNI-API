using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eUNI_API.Models.Entities.FieldOfStudy;

namespace eUNI_API.Models.Entities.Cache;

public class CalculatedClassesDate
{
    [Key]
    public int Id { get; set; }
    
    public int ClassId { get; set; }
    
    public DateOnly Date { get; set; }
    
    [ForeignKey("ClassId")]
    public Class Class { get; set; }
}