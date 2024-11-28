using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eUNI_API.Models.Entities.FieldOfStudy;

namespace eUNI_API.Models.Entities.OrganizationInfo;

public class Hour
{
    [Key]
    public int Id { get; set; }
    public byte StartHour { get; set; }
    public byte StartMinute { get; set; }
    public byte EndHour { get; set; }
    public byte EndMinute { get; set; }
    
    [InverseProperty("StartHour")]
    public IEnumerable<Class> ClassesStartHour { get; set; }
    
    [InverseProperty("EndHour")]
    public IEnumerable<Class> ClassesEndHour { get; set; }
}