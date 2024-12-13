using eUNI_API.Data;
using eUNI_API.Exception;
using eUNI_API.Helpers;
using eUNI_API.Models.Dto.Organization;
using eUNI_API.Models.Entities.OrganizationInfo;
using eUNI_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Repositories;

public class OrganizationRepository(AppDbContext context): IOrganizationRepository
{
    private readonly AppDbContext _context = context;

    public OrganizationOfTheYear GetOrganizationOfTheYear(int id)
    {
        var organizationOfTheYear = _context.OrganizationsOfTheYear
            .AsNoTracking()
            .FirstOrDefault(o => o.Id == id);
        if(organizationOfTheYear == null) throw new HttpNotFoundException($"No organization found with id {id}");
        return organizationOfTheYear;
    }
    
    public async Task<List<OrganizationOfTheYear>> GetYearOrganizations()
    {
        return await _context.OrganizationsOfTheYear
            .Include(o=>o.DayOffs)
            .ToListAsync();
    }

    public async Task CreateYearOrganization(AcademicYearDetails semesterDetails, YearOrganizationRequest yearOrganizationRequest)
    {
        if (semesterDetails.YearId == null || semesterDetails.FirstHalfOfYear == null) return;
        var newOrganization = new OrganizationOfTheYear
        {
            YearId = semesterDetails.YearId.Value,
            FirstHalfOfYear = semesterDetails.FirstHalfOfYear.Value,
            StartDay = yearOrganizationRequest.StartDate,
            EndDay = yearOrganizationRequest.EndDate
        };
        
        _context.OrganizationsOfTheYear.Add(newOrganization);
        if(yearOrganizationRequest.DaysOff.Count != 0)
            _context.DaysOff.AddRange(yearOrganizationRequest.DaysOff.Select(day => new DayOff
            {
                OrganizationsOfTheYear = newOrganization,
                Day = day
            }));
        
        await _context.SaveChangesAsync();
    }

    public async Task DeleteYearOrganization(int yearOrganizationId)
    {
        var organization = GetOrganizationOfTheYear(yearOrganizationId);
        var fieldsOfStudyLogs = await _context.FieldOfStudyLogs.Where(f=>f.OrganizationsOfTheYearId == yearOrganizationId).ToListAsync();
        
        if(fieldsOfStudyLogs.Count > 0)
            throw new HttpBadRequestHttpException("Some of the fields of study logs are not in the organization");
            
        _context.Remove(organization);
        await _context.SaveChangesAsync();
    }

    public async Task<OrganizationOfTheYear> GetOrganizationsInfo(int fieldOfStudyLogsId)
    {
        var fieldOfStudyLog = await _context.FieldOfStudyLogs
            .Include(f => f.OrganizationsOfTheYear)
            .FirstOrDefaultAsync(f => f.Id == fieldOfStudyLogsId);
        
        if(fieldOfStudyLog == null) throw new HttpNotFoundException($"Organization not found: id={fieldOfStudyLogsId}");
        
        return fieldOfStudyLog.OrganizationsOfTheYear;
    }
    
    public async Task<List<DateOnly>> GetDaysOff(int organizationId)
    {
        var organizationOfTheYear = GetOrganizationOfTheYear(organizationId);

        var daysOff = await _context.DaysOff
            .AsNoTracking()
            .Where(d=>d.OrganizationsOfTheYearId == organizationOfTheYear.Id)
            .Select(d=>d.Day)
            .ToListAsync();
        
        return daysOff;
    }

    public async Task UpdateYearOrganization(int organizationId, YearOrganizationRequest yearOrganizationRequest)
    {
        var organization = GetOrganizationOfTheYear(organizationId);
        organization.StartDay = yearOrganizationRequest.StartDate;
        organization.EndDay = yearOrganizationRequest.EndDate;
        await UpdateOrganizationDaysOff(organization, yearOrganizationRequest.DaysOff);
    }

    private async Task UpdateOrganizationDaysOff(OrganizationOfTheYear organization, List<DateOnly> daysOff)
    {
        var organizationDaysOff = await GetOrganizationDaysOff(organization.Id);
        if(daysOff.Count == 0)
        {
            _context.RemoveRange(organizationDaysOff);
            await _context.SaveChangesAsync();
            return;
        }
        
        var entityDifference = daysOff.Count - organizationDaysOff.Count;
        switch (entityDifference)
        {
            case > 0:
                var daysOffToAdd = new List<DayOff>();
                for(var i = 0; i < entityDifference; i++) daysOffToAdd.Add(new DayOff{OrganizationsOfTheYearId = organization.Id});
                _context.DaysOff.AddRange(daysOffToAdd);
                break;
            case < 0:
                var daysOffToRemove = organizationDaysOff.Slice(organizationDaysOff.Count + entityDifference - 1, -entityDifference);
                _context.DaysOff.RemoveRange(daysOffToRemove);
                break;
        }
        
        await _context.SaveChangesAsync();
        organizationDaysOff = await GetOrganizationDaysOff(organization.Id);
        
        var newDaysOff = organizationDaysOff.Select((dayOff, index) =>
        {
            dayOff.Day = daysOff[index];
            return dayOff;
        });
        _context.DaysOff.UpdateRange(newDaysOff);
        await _context.SaveChangesAsync();
    }

    public async Task<List<DayOff>> GetOrganizationDaysOff(int organizationId)
    {
        return await _context.DaysOff.Where(d => d.OrganizationsOfTheYearId == organizationId).ToListAsync();
    }

    public async Task<List<DayOff>> GetAllDaysOff()
    {
        return await _context.DaysOff.ToListAsync();
    }

    public async Task<OrganizationOfTheYear> GetNewestOrganization()
    {
        var organizations = await _context.OrganizationsOfTheYear
            .AsNoTracking()
            .ToListAsync();
        var newestOrganizationId = organizations.Max(organization => organization.Id);
        var newestAcademicOrganizations = await _context.OrganizationsOfTheYear
            .AsNoTracking()
            .Where(y => y.Id == newestOrganizationId)
            .ToListAsync();
        
        if (newestAcademicOrganizations.Count == 1) return newestAcademicOrganizations[0];
        
        var newestOrganization = newestAcademicOrganizations.FirstOrDefault(
            academicOrganization => academicOrganization.FirstHalfOfYear == false);
        
        if(newestOrganization == null) throw new HttpNotFoundException("Organization not found");
        return newestOrganization;
    }

    private async Task<List<OrganizationOfTheYear>> GetOrganizationsOfTheYear()
    {
        return await _context.OrganizationsOfTheYear
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<OrganizationOfTheYear?> GetOrganizationToUpgrade()
    {
        var newestOrganization = await GetNewestOrganization();
        var organizations = await GetOrganizationsOfTheYear();

        if (newestOrganization.FirstHalfOfYear == false)
            return organizations.FirstOrDefault(o => 
                o.YearId == newestOrganization.YearId && 
                o.FirstHalfOfYear);
        
        var previousYear = await GetPreviousYear(newestOrganization.YearId);

        return previousYear == null ? 
            null : 
            organizations.First(o=> o.YearId == previousYear.Id && o.FirstHalfOfYear == false);
    }

    public async Task<List<Year>> GetYears()
    {
        return await _context.Years
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Year?> GetPreviousYear(int yearId)
    {
        var years = await GetYears();
        var currentYear = years.FirstOrDefault(y => y.Id == yearId);
        
        if(currentYear == null) throw new HttpNotFoundException($"Year not found: id={yearId}");
        
        var prevYearName = OrganizationHelper.GetPrevYearName(currentYear.Name);
        return years.FirstOrDefault(y => y.Name == prevYearName);
    }

    public async Task<Year> GetOrCreateNextYear(int yearId)
    {
        var years = await GetYears();
        var year = years.First(year => year.Id == yearId);
        var nextYearName = OrganizationHelper.GetNextYearName(year.Name);
        var nextYear = years.FirstOrDefault(y => y.Name == nextYearName);

        if (nextYear != null) return nextYear;
        
        var newYear = new Year
        {
            Name = nextYearName, 
        };
        _context.Years.Add(newYear);
        await _context.SaveChangesAsync();
        return newYear;
    }
}