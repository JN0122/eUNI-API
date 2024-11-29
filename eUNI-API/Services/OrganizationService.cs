using eUNI_API.Data;
using eUNI_API.Models.Dto.Organization;
using eUNI_API.Models.Entities.OrganizationInfo;
using eUNI_API.Repositories.Interfaces;
using eUNI_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Services;

public class OrganizationService(AppDbContext appDbContext, IOrganizationRepository organizationRepository): IOrganizationService
{
    private readonly IOrganizationRepository _organizationRepository = organizationRepository;
    public Task<List<YearOrganization>> GetYearOrganizations()
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

    public Task<List<YearOrganization>> GetYears()
    {
        throw new NotImplementedException();
    }

    public Task CreateYear(YearRequest yearRequest)
    {
        throw new NotImplementedException();
    }

    public Task UpdateYear(int id, YearRequest yearRequest)
    {
        throw new NotImplementedException();
    }

    public Task DeleteYear(int id)
    {
        throw new NotImplementedException();
    }
}