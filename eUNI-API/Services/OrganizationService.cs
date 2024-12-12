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

    public async Task CreateYearOrganization(YearOrganizationRequest yearOrganizationRequest)
    {
        var nextSemesterDetails = await GetNextSemesterDetails();
        await _organizationRepository.CreateYearOrganization(nextSemesterDetails, yearOrganizationRequest);
    }

    public async Task UpdateYearOrganization(int id, YearOrganizationRequest yearOrganizationRequest)
    {
        await _organizationRepository.UpdateYearOrganization(id, yearOrganizationRequest);
    }

    public async Task DeleteYearOrganization(int id)
    {
        await _organizationRepository.DeleteYearOrganization(id);
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

    public async Task<AcademicYearDetails> GetNextSemesterDetails()
    {
        var newestOrganization = await _organizationRepository.GetNewestOrganization();
        if (newestOrganization.FirstHalfOfYear)
            return new AcademicYearDetails
            {
                YearId = newestOrganization.YearId,
                FirstHalfOfYear = false,
            };

        var newYear = await _organizationRepository.GetOrCreateNextYear(newestOrganization.YearId);
        return new AcademicYearDetails
        {
            YearId = newYear.Id,
            FirstHalfOfYear = true
        };
    }

    public async Task<AcademicYearDetails> GetSemesterDetailsToUpgrade()
    {
        var newestOrganization = await _organizationRepository.GetNewestOrganization();
        
        if(newestOrganization.FirstHalfOfYear == false)
            return new AcademicYearDetails
            {
                YearId = newestOrganization.YearId,
                FirstHalfOfYear = true
            };
        
        var previousYear = await _organizationRepository.GetPreviousYear(newestOrganization.YearId);
        
        if(previousYear == null) 
            return new AcademicYearDetails
            {
                YearId = newestOrganization.YearId,
                FirstHalfOfYear = newestOrganization.FirstHalfOfYear
            };
        
        return new AcademicYearDetails
        {
            YearId = previousYear.Id,
            FirstHalfOfYear = false
        };
    }
}