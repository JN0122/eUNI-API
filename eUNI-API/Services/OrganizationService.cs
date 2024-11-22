using eUNI_API.Data;
using eUNI_API.Models.Dto.Organization;
using eUNI_API.Models.Entities.OrganizationInfo;
using eUNI_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Services;

public class OrganizationService(AppDbContext appDbContext): IOrganizationService
{
    public Task<List<YearOrganization>> GetYearOrganizations()
    {
        throw new NotImplementedException();
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