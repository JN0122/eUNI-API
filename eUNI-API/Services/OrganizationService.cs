using eUNI_API.Data;
using eUNI_API.Models.Dto.Organization;
using eUNI_API.Repositories.Interfaces;
using eUNI_API.Services.Interfaces;

namespace eUNI_API.Services;

public class OrganizationService(IOrganizationRepository organizationRepository): IOrganizationService
{
    private readonly IOrganizationRepository _organizationRepository = organizationRepository;
    public async Task<List<YearOrganization>> GetYearOrganizations()
    {
        var organizations = await _organizationRepository.GetYearOrganizations();
        var daysOff = await _organizationRepository.GetAllDaysOff();
        
        return organizations.Select(o=> new YearOrganization
        {
            Id = o.Id,
            YearId = o.YearId,
            FirstHalfOfYear = o.FirstHalfOfYear,
            StartDate = o.StartDay,
            EndDate = o.EndDay,
            DaysOff = daysOff.Where(d=>d.OrganizationsOfTheYearId == o.Id).Select(dayOff => dayOff.Day).ToList(),
        }).ToList();
    }

    public Task CreateYearOrganization(YearOrganizationRequest yearOrganizationRequest)
    {
        throw new NotImplementedException();
    }

    public Task UpdateYearOrganization(int id, YearOrganizationRequest yearOrganizationRequest)
    {
        throw new NotImplementedException();
    }

    public Task DeleteYearOrganization(int id)
    {
        throw new NotImplementedException();
    }
    
    public AcademicYearDaysOff GetAcademicYearDaysOff(int fieldOfStudyLogId)
    {
        var organizationsInfo = _organizationRepository.GetOrganizationsInfo(fieldOfStudyLogId).Result;
        var daysOff = _organizationRepository.GetDaysOff(organizationsInfo.Id).Result;
        return new AcademicYearDaysOff
        {
            StartYearDate = organizationsInfo.StartDay,
            EndYearDate = organizationsInfo.EndDay,
            DaysOff = daysOff,
        };
    }

    public async Task<List<YearDto>> GetYears()
    {
        var years = await organizationRepository.GetYears();
        return years.Select((year) => new YearDto
        {
            Id = year.Id,
            Name = year.Name,
        }).ToList();
    }

    public async Task<NextAcademicYear> GetNextSemesterDetails()
    {
        var newestOrganization = await _organizationRepository.GetNewestOrganization();
        Console.WriteLine(newestOrganization.Id);
        if (newestOrganization.FirstHalfOfYear)
            return new NextAcademicYear
            {
                YearId = newestOrganization.YearId,
                FirstHalfOfYear = false,
            };

        var newYear = await _organizationRepository.GetOrCreateNextYear(newestOrganization.YearId);
        return new NextAcademicYear
        {
            YearId = newYear.Id,
            FirstHalfOfYear = true
        };
    }
}