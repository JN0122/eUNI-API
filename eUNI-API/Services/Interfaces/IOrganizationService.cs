using eUNI_API.Models.Dto.Organization;
using eUNI_API.Models.Entities.OrganizationInfo;

namespace eUNI_API.Services.Interfaces;

public interface IOrganizationService
{
    public Task<OrganizationOfTheYear> GetOrganizationsInfo(int fieldOfStudyLogsId);
    public Task<IEnumerable<DateOnly>> GetDaysOff(int organizationId);
    public int GetNewestOrganizationId();
    public Task<List<YearOrganization>> GetYearOrganizations();
    public Task CreateYearOrganization(YearOrganizationRequest yearOrganizationRequest);
    public Task UpdateYearOrganization(int id, YearOrganizationRequest yearOrganizationRequest);
    public Task DeleteYearOrganization(int id);
}