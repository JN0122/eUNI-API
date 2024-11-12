using eUNI_API.Data;
using eUNI_API.Enums;
using eUNI_API.Helpers;
using eUNI_API.Models.Dto;
using eUNI_API.Models.Entities.FieldOfStudy;
using eUNI_API.Models.Entities.OrganizationInfo;
using eUNI_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Services;

public class ScheduleService(AppDbContext context): IScheduleService
{
    private readonly AppDbContext _context = context;

    public async Task<OrganizationOfTheYear> GetOrganizationsInfo(int organizationId)
    {
        var organizationOfTheYear = await _context.OrganizationsOfTheYear
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == organizationId);
        
        if(organizationOfTheYear == null) throw new ArgumentException("Organization not found");
        
        return organizationOfTheYear;
    }

    public async Task<Class> GetClass(int classId)
    {
        var classEntity = await _context.Classes
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == classId);
        
        if (classEntity == null) throw new ArgumentException("Class not found");
        
        return classEntity;
    }
    
    public List<DateOnly> CalculateDates(DateOnly yearStart, DateOnly yearEnd, WeekDay classWeekDay, 
        int repeatClassInDays, bool startFirstWeek)
    {
        var date = yearStart.AddDays((int)classWeekDay - (int)ConvertDay.ToWeekDay(yearStart.DayOfWeek));
        
        if(!startFirstWeek)
            date = date.AddDays(7);
        
        if (date < yearStart)
            date = date.AddDays(repeatClassInDays);
        
        var dates = new List<DateOnly> { date };
        for (; date < yearEnd; date = date.AddDays(repeatClassInDays))
            dates.Add(date);

        return dates;
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
    
    public async Task<List<DateOnly>> CalculateClassesDates(ClassesToCalculateDto classesToCalculateDto)
    {
        var organizationInfo = await GetOrganizationsInfo(classesToCalculateDto.OrganizationsOfTheYearId);
        var classEntity = await GetClass(classesToCalculateDto.ClassId);
        
        if (classEntity.WeekDay == null) throw new ArgumentException("Cannot calculate classes when week day is null");
        
        var repeatClassInDays = classEntity.IsOddWeek == null ? 7 : 14;
        var startFirstWeek = classEntity.IsOddWeek ?? true;

        var dates = CalculateDates
        (
            organizationInfo.StartDay, 
            organizationInfo.EndDay, 
            classEntity.WeekDay.Value, 
            repeatClassInDays, 
            startFirstWeek
        );
        
        foreach (var dayOff in await GetDaysOff(classesToCalculateDto.OrganizationsOfTheYearId))
        {
            dates.Remove(dayOff);
        }

        return dates;
    }
}