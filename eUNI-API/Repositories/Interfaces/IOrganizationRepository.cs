using eUNI_API.Models.Entities.OrganizationInfo;

namespace eUNI_API.Repositories.Interfaces;

public interface IOrganizationRepository
{
    public Task<OrganizationOfTheYear> GetOrganizationsInfo(int fieldOfStudyLogsId);
    public Task<IEnumerable<DateOnly>> GetDaysOff(int organizationId);
    public Task<OrganizationOfTheYear> GetNewestOrganization();
    public Task<List<Year>> GetYears();
    public Task<Year> GetOrCreateNextYear(int yearId);
}