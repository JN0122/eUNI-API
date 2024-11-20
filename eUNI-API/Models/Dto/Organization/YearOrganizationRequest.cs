using System.ComponentModel.DataAnnotations;

namespace eUNI_API.Models.Dto.Organization;

public class YearOrganizationRequest
{
    public int YearId { get; set; }
    
    public bool FirstHalfOfYear { get; set; }
    
    public DateOnly StartDate { get; set; }
    
    public DateOnly EndDate { get; set; }
    
    public List<DateOnly>? DatesOff { get; set; }
}