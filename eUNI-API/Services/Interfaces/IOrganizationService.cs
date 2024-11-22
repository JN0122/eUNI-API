using eUNI_API.Models.Dto.Organization;
using eUNI_API.Models.Entities.OrganizationInfo;

namespace eUNI_API.Services.Interfaces;

public interface IOrganizationService
{
    public Task<List<YearOrganization>> GetYearOrganizations();
    public Task CreateYearOrganization(YearOrganizationRequest yearOrganizationRequest);
    public Task UpdateYearOrganization(int id, YearOrganizationRequest yearOrganizationRequest);
    public Task DeleteYearOrganization(int id);
    public Task<List<YearOrganization>> GetYears();
    public Task CreateYear(YearRequest yearRequest);
    public Task UpdateYear(int id, YearRequest yearRequest);
    public Task DeleteYear(int id);
}