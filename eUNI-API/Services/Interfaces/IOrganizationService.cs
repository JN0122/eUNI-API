using eUNI_API.Models.Entities.OrganizationInfo;

namespace eUNI_API.Services.Interfaces;

public interface IOrganizationService
{
    public Task<OrganizationOfTheYear> GetOrganizationsInfo(int fieldOfStudyLogsId);
    public Task<IEnumerable<DateOnly>> GetDaysOff(int organizationId);
    public int GetNewestOrganizationId();
}