using eUNI_API.Data;
using eUNI_API.Models.Dto.Organization;
using eUNI_API.Models.Entities.OrganizationInfo;
using eUNI_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Services;

public class OrganizationService(AppDbContext appDbContext): IOrganizationService
{
    private readonly AppDbContext _context = appDbContext;
    
    public async Task<OrganizationOfTheYear> GetOrganizationsInfo(int fieldOfStudyLogsId)
    {
        var fieldOfStudyLog = await _context.FieldOfStudyLogs
            .Include(f => f.OrganizationsOfTheYear)
            .FirstOrDefaultAsync(f => f.Id == fieldOfStudyLogsId);
        
        if(fieldOfStudyLog == null) throw new ArgumentException("Organization not found");
        
        return fieldOfStudyLog.OrganizationsOfTheYear;
    }
    
    public async Task<IEnumerable<DateOnly>> GetDaysOff(int organizationId)
    {
        var organizationOfTheYear = await GetOrganizationsInfo(organizationId);

        var daysOff = _context.DaysOff
            .Where(d=>d.OrganizationsOfTheYearId == organizationOfTheYear.Id)
            .Select(d=>d.Day)
            .ToList();
        
        return daysOff;
    }

    public int GetNewestOrganizationId()
    {
        var yearMaxId = _context.Years.Max(year => year.Id);
        var newestAcademicOrganizationId = _context.OrganizationsOfTheYear.FirstOrDefault(y => y.Id == yearMaxId)?.Id;
        if(newestAcademicOrganizationId == null) throw new ArgumentException("Organization not found");
        return newestAcademicOrganizationId.Value;
    }

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