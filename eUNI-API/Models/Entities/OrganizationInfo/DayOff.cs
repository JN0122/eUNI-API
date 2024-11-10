using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eUNI_API.Models.Entities.OrganizationInfo;

public class DayOff
{
    [Key]
    public int Id { get; set; }
    
    public int OrganizationsOfTheYearId { get; set; }
    
    public DateOnly Day { get; set; }
    
    [ForeignKey("OrganizationsOfTheYearId")]
    public OrganizationOfTheYear OrganizationsOfTheYear { get; set; }
}