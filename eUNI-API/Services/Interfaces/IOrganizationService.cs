using eUNI_API.Models.Dto.Organization;
using eUNI_API.Models.Entities.OrganizationInfo;

namespace eUNI_API.Services.Interfaces;

public interface IOrganizationService
{
    public Task<List<YearOrganization>> GetYearOrganizations();
    public AcademicYearDaysOff GetAcademicYearDaysOff(int fieldOfStudyLogId);
    public Task CreateYearOrganization(YearOrganizationRequest yearOrganizationRequest);
    public Task UpdateYearOrganization(int id, YearOrganizationRequest yearOrganizationRequest);
    public Task DeleteYearOrganization(int id);
    public Task<List<YearDto>> GetYears();
    public Task<NextAcademicYear> GetNextSemesterDetails();
}