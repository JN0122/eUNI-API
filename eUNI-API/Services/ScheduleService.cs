using eUNI_API.Data;
using eUNI_API.Enums;
using eUNI_API.Helpers;
using eUNI_API.Models.Dto.Classes;
using eUNI_API.Models.Dto.Schedule;
using eUNI_API.Models.Entities.FieldOfStudy;
using eUNI_API.Models.Entities.OrganizationInfo;
using eUNI_API.Repositories.Interfaces;
using eUNI_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Services;

public class ScheduleService(AppDbContext context, IOrganizationRepository organizationRepository, IClassesRepository classesRepository, IGroupRepository groupRepository): IScheduleService
{
    private readonly AppDbContext _context = context;
    private readonly IOrganizationRepository _organizationRepository = organizationRepository;
    private readonly IClassesRepository _classesRepository = classesRepository;
    private readonly IGroupRepository _groupRepository = groupRepository;

    private class ThisWeekClass
    {
        public Class classEntity { get; set; }
        public DateOnly date { get; set; }
    }
    
    private async Task<DateOnly?> CalculateClassDate(
        Class classEntity, 
        OrganizationOfTheYear organizationInfo, 
        DateOnly startDay, 
        DateOnly endDay)
    {
        if (classEntity.WeekDay == null) throw new ArgumentException("Cannot calculate classes when week day is null");
        if (organizationInfo.StartDay > startDay) return null;
        
        var repeatClassInDays = classEntity.IsOddWeek == null ? 7 : 14;
        var startFirstWeek = classEntity.IsOddWeek ?? true;

        var date = DateHelper.CalculateDate
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
        
        var daysOff = await _organizationRepository.GetDaysOff(organizationInfo.Id);

        return daysOff.Any(dayOff => dayOff == date) ? null : date;
    }

    private async Task<List<Class>> GetClasses(int fieldOfStudyLogId)
    {
        var classes = await _context.Classes
            .Where(c=>c.FieldOfStudyLogId == fieldOfStudyLogId)
            .Include(c=>c.Group)
            .ToListAsync();
        
        return classes;
    }

    public IEnumerable<HourDto> GetHours()
    {
        return _classesRepository.GetHour();
    }

    public ClassAssignment? GetClassAssigment(int classId, DateOnly date)
    {
        var assignment = _context.Assignments.FirstOrDefault(a => a.Id == classId && a.DeadlineDate == date);
        
        return assignment == null ? null : new ClassAssignment
        {
            Name = assignment.Name
        };
    }

    public int GetGroupType(int groupId)
    {
        var group = _context.Groups.First(g => g.Id == groupId);
        return group.Type;
    }

    public async Task<ScheduleDto> GetSchedule(ScheduleInfoRequestDto scheduleInfoRequest)
    {
        var classes = await GetClasses(scheduleInfoRequest.fieldOfStudyLogId);
        var allUserClasses = classes.Where(c => scheduleInfoRequest.GroupIds.Any(groupId => groupId == c.GroupId));
        var organizationInfo = await _organizationRepository.GetOrganizationsInfo(scheduleInfoRequest.fieldOfStudyLogId);
        var (startOfWeek, endOfWeek) = DateHelper.GetWeekStartAndEndDates(scheduleInfoRequest.Year, scheduleInfoRequest.WeekNumber);
        
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
                Id = hour.HourId,
                Hour = hour.HourName
            };
            foreach (var thisWeekClass in thisWeekClasses)
            {
                if(thisWeekClass.classEntity.StartHourId != hour.HourId) continue;
                var assignment = GetClassAssigment(thisWeekClass.classEntity.Id, thisWeekClass.date);
                var weekDay = DateHelper.GetWeekDay(thisWeekClass.date);
                var prop = weekDays.GetType().GetProperty(weekDay);
                var group = _groupRepository.GetGroupName(thisWeekClass.classEntity.Id);
                
                if(prop == null) throw new Exception($"Property '{weekDay}' not found in ScheduleWeekDays");
                prop.SetValue(weekDays, new ScheduleClass
                {
                    Hours = thisWeekClass.classEntity.EndHourId - thisWeekClass.classEntity.StartHourId + 1,
                    Name = $"{thisWeekClass.classEntity.Name} ({group})",
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