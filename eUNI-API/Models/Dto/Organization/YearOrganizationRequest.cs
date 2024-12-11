using System.ComponentModel.DataAnnotations;

namespace eUNI_API.Models.Dto.Organization;

public class YearOrganizationRequest
{
    [Required]
    public DateOnly StartDate { get; set; }
    
    [Required]
    public DateOnly EndDate { get; set; }

    [Required] 
    public List<DateOnly> DaysOff { get; set; }
}