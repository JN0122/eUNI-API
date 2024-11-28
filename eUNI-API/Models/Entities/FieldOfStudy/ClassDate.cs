using System.ComponentModel.DataAnnotations.Schema;

namespace eUNI_API.Models.Entities.FieldOfStudy;

public class ClassDate
{
    public int Id { get; set; }
    public int ClassId { get; set; }
    public DateOnly Date { get; set; }
    
    [ForeignKey("ClassId")]
    public Class Class { get; set; }
}