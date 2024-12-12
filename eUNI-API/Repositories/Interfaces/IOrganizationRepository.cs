using eUNI_API.Models.Dto.Organization;
using eUNI_API.Models.Entities.OrganizationInfo;

namespace eUNI_API.Repositories.Interfaces;

public interface IOrganizationRepository
{
    public OrganizationOfTheYear GetOrganizationOfTheYear(int id);
    public Task<List<OrganizationOfTheYear>> GetYearOrganizations();
    public Task CreateYearOrganization(AcademicYearDetails semesterDetails, YearOrganizationRequest yearOrganizationRequest);
    public Task DeleteYearOrganization(int yearOrganizationId);
    public Task<OrganizationOfTheYear> GetOrganizationsInfo(int fieldOfStudyLogsId);
    public Task<List<DateOnly>> GetDaysOff(int organizationId);
    public Task UpdateYearOrganization(int organizationId, YearOrganizationRequest yearOrganizationRequest);
    public Task<List<DayOff>> GetAllDaysOff();
    public Task<OrganizationOfTheYear> GetNewestOrganization();
    public Task<OrganizationOfTheYear?> GetOrganizationToUpgrade();
    public Task<List<Year>> GetYears();
    public Task<Year?> GetPreviousYear(int yearId);
    public Task<Year> GetOrCreateNextYear(int yearId);
}