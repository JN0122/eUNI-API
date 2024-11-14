using eUNI_API.Data;
using eUNI_API.Enums;
using eUNI_API.Helpers;
using eUNI_API.Models.Dto;
using eUNI_API.Models.Dto.Schedule;
using eUNI_API.Models.Entities.FieldOfStudy;
using eUNI_API.Models.Entities.OrganizationInfo;
using eUNI_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Services;

public class ScheduleService(AppDbContext context): IScheduleService
{
    private readonly AppDbContext _context = context;

    private class ThisWeekClass
    {
        public Class classEntity { get; set; }
        public DateOnly date { get; set; }
    }
    
    private async Task<OrganizationOfTheYear> GetOrganizationsInfo(int fieldOfStudyLogsId)
    {
        var fieldOfStudyLog = await _context.FieldOfStudyLogs
            .AsNoTracking()
            .Include(f => f.OrganizationsOfTheYear)
            .FirstOrDefaultAsync(f => f.Id == fieldOfStudyLogsId);
        
        if(fieldOfStudyLog == null) throw new ArgumentException("Organization not found");
        
        return fieldOfStudyLog.OrganizationsOfTheYear;
    }
    
    private static DateOnly? CalculateDate(DateOnly yearStart, DateOnly yearEnd, WeekDay classWeekDay, 
        DateOnly startDay, DateOnly endDay, int repeatClassInDays, bool startFirstWeek)
    {
        var date = yearStart.AddDays((int)classWeekDay - (int)ConvertDay.ToWeekDay(yearStart.DayOfWeek));
        
        if(!startFirstWeek)
            date = date.AddDays(7);
        
        if (date < yearStart)
            date = date.AddDays(repeatClassInDays);
        
        var dates = new List<DateOnly> { date };
        for (; date <= yearEnd && date <= endDay; date = date.AddDays(repeatClassInDays))
            dates.Add(date);
        
        return dates.Last() < startDay? null : dates.Last();
    }

    private async Task<IEnumerable<DateOnly>> GetDaysOff(int organizationId)
    {
        var organizationOfTheYear = await GetOrganizationsInfo(organizationId);

        var daysOff = _context.DaysOff
            .Where(d=>d.OrganizationsOfTheYearId == organizationOfTheYear.Id)
            .Select(d=>d.Day)
            .ToList();
        
        return daysOff;
    }
    
    private async Task<DateOnly?> CalculateClassDate(
        Class classEntity, 
        OrganizationOfTheYear organizationInfo, 
        DateOnly startDay, 
        DateOnly endDay)
    {
        if (classEntity.WeekDay == null) throw new ArgumentException("Cannot calculate classes when week day is null");
        
        var repeatClassInDays = classEntity.IsOddWeek == null ? 7 : 14;
        var startFirstWeek = classEntity.IsOddWeek ?? true;

        var date = CalculateDate
        (
            organizationInfo.StartDay, 
            organizationInfo.EndDay,
            classEntity.WeekDay.Value, 
            startDay,
            endDay,
            repeatClassInDays, 
            startFirstWeek
        );
        if(date == null) return null;
        
        var daysOff = await GetDaysOff(organizationInfo.Id);

        return daysOff.Any(dayOff => dayOff == date) ? null : date;
    }

    private static (DateOnly StartOfWeek, DateOnly EndOfWeek) GetWeekStartAndEndDates(int year, int weekNumber)
    {
        var firstDayOfYear = new DateOnly(year, 1, 1);
        var firstWeekStart = firstDayOfYear.AddDays(DayOfWeek.Monday - firstDayOfYear.DayOfWeek);
        var startOfWeek = firstWeekStart.AddDays((weekNumber - 1) * 7);
        
        return (StartOfWeek: startOfWeek, EndOfWeek: startOfWeek.AddDays(6));
    }

    private async Task<List<Class>> GetClasses(int fieldOfStudyLogId)
    {
        var classes = await _context.Classes
            .Where(c=>c.FieldOfStudyLogId == fieldOfStudyLogId)
            .ToListAsync();
        
        return classes;
    }

    private List<Hour> GetHours()
    {
        return _context.Hours.ToList();
    }

    private ClassAssignment? GetClassAssigment(int classId, DateOnly date)
    {
        var assignment = _context.Assignments.FirstOrDefault(a => a.Id == classId && a.DeadlineDate == date);
        
        return assignment == null ? null : new ClassAssignment
        {
            Name = assignment.Name
        };
    }

    private static string GetWeekDay(DateOnly date)
    {
        return ConvertDay.ToWeekDay(date.DayOfWeek).ToString();
    }

    private int GetGroupType(int groupId)
    {
        var group = _context.Groups.First(g => g.Id == groupId);
        return group.Type;
    }

    public async Task<ScheduleDto> GetSchedule(ScheduleInfoDto scheduleInfo)
    {
        var classes = await GetClasses(scheduleInfo.fieldOfStudyLogId);
        var allUserClasses = classes.Where(c => scheduleInfo.GroupIds.Any(groupId => groupId == c.GroupId));
        var organizationInfo = await GetOrganizationsInfo(scheduleInfo.fieldOfStudyLogId);
        var (startOfWeek, endOfWeek) = GetWeekStartAndEndDates(scheduleInfo.Year, scheduleInfo.WeekNumber);
        
        var thisWeekClasses = new List<ThisWeekClass>();
        
        foreach (var userClass in allUserClasses)
        {
            var date = await CalculateClassDate(userClass, organizationInfo, startOfWeek, endOfWeek);
            if(date == null) continue;
            thisWeekClasses.Add(new ThisWeekClass
            {
                classEntity = userClass,
                date = date.Value
            });
        }

        var schedule = new ScheduleDto
        {
            Date = $"{startOfWeek} - {endOfWeek}"
        };
        foreach(var hour in GetHours())
        {
            var weekDays = new ScheduleWeekDays
            {
                Id = hour.Id,
                Hour = hour.HourInterval
            };
            foreach (var thisWeekClass in thisWeekClasses)
            {
                if(thisWeekClass.classEntity.StartHour != hour) continue;
                var assignment = GetClassAssigment(thisWeekClass.classEntity.Id, thisWeekClass.date);
                var weekDay = GetWeekDay(thisWeekClass.date);
                var prop = weekDays.GetType().GetProperty(weekDay);
                
                if(prop == null) throw new Exception($"Property '{weekDay}' not found in ScheduleWeekDays");
                prop.SetValue(weekDays, new ScheduleClass
                {
                    Hours = thisWeekClass.classEntity.EndHourId - thisWeekClass.classEntity.StartHourId + 1,
                    Name = thisWeekClass.classEntity.Name,
                    Room = thisWeekClass.classEntity.Room,
                    Type = GetGroupType(thisWeekClass.classEntity.GroupId),
                    Assignment = assignment
                });
            }
            schedule.Schedule.Add(weekDays);
        }

        return schedule;
    }
}