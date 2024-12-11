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

    public int GetNewestOrganizationId()
    {
        var yearMaxId = _context.Years.Max(year => year.Id);
        var newestAcademicOrganizationId = _context.OrganizationsOfTheYear.FirstOrDefault(y => y.Id == yearMaxId)?.Id;
        if(newestAcademicOrganizationId == null) throw new ArgumentException("Organization not found");
        return newestAcademicOrganizationId.Value;
    }

    public async Task<List<Year>> GetYears()
    {
        return await _context.Years.ToListAsync();
    }
}