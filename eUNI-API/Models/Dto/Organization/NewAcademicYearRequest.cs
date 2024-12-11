using System.ComponentModel.DataAnnotations;

namespace eUNI_API.Models.Dto.Organization;

public class NewAcademicYearRequest
{
    [Required]
    public DateTime StartDate { get; set; }
    
    [Required]
    public DateTime EndDate { get; set; }
    
    [Required]
    public IEnumerable<DateTime>? DaysOff { get; set; }
}