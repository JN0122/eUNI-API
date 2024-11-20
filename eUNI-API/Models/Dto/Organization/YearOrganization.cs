namespace eUNI_API.Models.Dto.Organization;

public class YearOrganization
{
    public int Id { get; set; }
    
    public int YearId { get; set; }
    
    public string YearName { get; set; }

    public bool FirstHalfOfYear { get; set; }

    public DateOnly StartDate { get; set; }
    
    public DateOnly EndDate { get; set; }
    
    public List<DateOnly>? DatesOff { get; set; }
}