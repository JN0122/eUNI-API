using System.ComponentModel.DataAnnotations;
using eUNI_API.Models.Entities.FieldOfStudy;

namespace eUNI_API.Models.Entities.Student;

public class Group
{
    [Key]
    public int Id { get; set; }

    [MaxLength(20)]
    public string Abbr { get; set; }
    
    public int Type { get; set; }
    
    public IEnumerable<Class> Classes { get; set; }
    public IEnumerable<StudentGroup> StudentGroups { get; set; }
}