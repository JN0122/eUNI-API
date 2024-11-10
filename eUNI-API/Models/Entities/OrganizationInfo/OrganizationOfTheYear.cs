using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eUNI_API.Models.Entities.FieldOfStudy;

namespace eUNI_API.Models.Entities.OrganizationInfo;

public class OrganizationOfTheYear
{
    [Key]
    public int Id { get; set; }

    public int YearId { get; set; }
    
    public bool FirstHalfOfYear { get; set; }

    public DateOnly StartDay { get; set; }
    
    public DateOnly EndDay { get; set; }
    
    [ForeignKey("YearId")]
    public Year Year { get; set; }
    
    public IEnumerable<FieldOfStudyLog> FieldOfStudyLogs { get; set; }
    public IEnumerable<DayOff>? DayOffs { get; set; }
}