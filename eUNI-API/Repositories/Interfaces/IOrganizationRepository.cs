using eUNI_API.Models.Entities.OrganizationInfo;

namespace eUNI_API.Repositories.Interfaces;

public interface IOrganizationRepository
{
    public Task<List<OrganizationOfTheYear>> GetYearOrganizations();
    public Task<OrganizationOfTheYear> GetOrganizationsInfo(int fieldOfStudyLogsId);
    public Task<List<DateOnly>> GetDaysOff(int organizationId);
    public Task<List<DayOff>> GetAllDaysOff();
    public Task<OrganizationOfTheYear> GetNewestOrganization();
    public Task<List<Year>> GetYears();
    public Task<Year> GetOrCreateNextYear(int yearId);
}