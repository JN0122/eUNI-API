namespace eUNI_API.Models.Dto.Organization;

public class AcademicYearDaysOff
{
    public DateOnly StartYearDate { get; set; }
    public DateOnly EndYearDate { get; set; }
    public IEnumerable<DateOnly> DaysOff { get; set; }
}