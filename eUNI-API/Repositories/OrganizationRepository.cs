using eUNI_API.Data;
using eUNI_API.Models.Entities.OrganizationInfo;
using eUNI_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Repositories;

public class OrganizationRepository(AppDbContext context): IOrganizationRepository
{
    private readonly AppDbContext _context = context;
    
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

    public async Task<OrganizationOfTheYear> GetNewestOrganization()
    {
        var organizations = await _context.OrganizationsOfTheYear.ToListAsync();
        var newestOrganizationId = organizations.Max(organization => organization.Id);
        var newestAcademicOrganizations = await _context.OrganizationsOfTheYear
            .Where(y => y.Id == newestOrganizationId)
            .ToListAsync();
        
        if (newestAcademicOrganizations.Count == 1) return newestAcademicOrganizations[0];
        
        var newestOrganization = newestAcademicOrganizations.FirstOrDefault(
            academicOrganization => academicOrganization.FirstHalfOfYear == false);
        
        if(newestOrganization == null) throw new ArgumentException("Organization not found");
        return newestOrganization;
    }

    public async Task<List<Year>> GetYears()
    {
        return await _context.Years.ToListAsync();
    }
}