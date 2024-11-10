using System.ComponentModel.DataAnnotations;
using eUNI_API.Models.Entities.FieldOfStudy;

namespace eUNI_API.Models.Entities.OrganizationInfo;

public class Hour
{
    [Key]
    public int Id { get; set; }
    
    [MaxLength(20)]
    public string HourInterval { get; set; }
    
    public IEnumerable<Class>? Classes { get; set; }
}