using eUNI_API.Data;
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
}