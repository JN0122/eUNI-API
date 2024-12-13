using eUNI_API.Data;
using eUNI_API.Enums;
using eUNI_API.Exception;
using eUNI_API.Helpers;
using eUNI_API.Models.Dto.Classes;
using eUNI_API.Models.Dto.Schedule;
using eUNI_API.Models.Entities.FieldOfStudy;
using eUNI_API.Models.Entities.OrganizationInfo;
using eUNI_API.Repositories.Interfaces;
using eUNI_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Services;

public class ScheduleService(AppDbContext context, IOrganizationRepository organizationRepository, 
    IClassesRepository classesRepository, IGroupRepository groupRepository,
    IHourRepository hourRepository, IFieldOfStudyRepository fieldOfStudyRepository,
    ICalendarRepository calendarRepository): IScheduleService
{
    private readonly AppDbContext _context = context;
    private readonly IOrganizationRepository _organizationRepository = organizationRepository;
    private readonly IClassesRepository _classesRepository = classesRepository;
    private readonly IGroupRepository _groupRepository = groupRepository;
    private readonly IHourRepository _hourRepository = hourRepository;
    private readonly IFieldOfStudyRepository _fieldOfStudyRepository = fieldOfStudyRepository;
    private readonly ICalendarRepository _calendarRepository = calendarRepository;

    private class ThisWeekClass
    {
        public Class classEntity { get; set; }
        public DateOnly date { get; set; }
    }

    public IEnumerable<HourDto> GetHours()
    {
        return _classesRepository.GetHour();
    }
    
    public int GetGroupType(int groupId)
    {
        var group = _context.Groups.First(g => g.Id == groupId);
        return group.Type;
    }

    public string GetGroupCalendarPath(int fieldOfStudyLogId, int groupId)
    {
        var fieldOfStudyInfo = _fieldOfStudyRepository.GetFieldOfStudyInfo(fieldOfStudyLogId);
        var group = _groupRepository.GetGroupById(groupId);
        return _calendarRepository.GetCalendarFilePath(fieldOfStudyInfo, _groupRepository.GetGroupName(fieldOfStudyLogId, group));
    }

    private List<Hour> GetClassesHours(List<Class> classes)
    {
        if(classes.Count == 0) return [];
        var startHourId = classes.Min(c => c.StartHourId);
        var endHourId = classes.Max(c => c.EndHourId);
        return _hourRepository.GetHoursRange(startHourId, endHourId).ToList();
    }

    private List<ThisWeekClass> FilterClassesBetweenDates(List<Class> classes, DateOnly startOfWeek, DateOnly endOfWeek)
    {
        if(classes.Count == 0) return [];
        var filteredClasses = new List<ThisWeekClass>();
        foreach (var classEntity in classes)
        {
            var date = _classesRepository.GetClassDates(classEntity.Id)
                .Where(cd => cd.Date >= startOfWeek && cd.Date <= endOfWeek);
            if(!date.Any()) continue;
            filteredClasses.AddRange(date.Select(d => new ThisWeekClass
            {
                classEntity = classEntity,
                date = d.Date
            }));
        }
        return filteredClasses;
    }

    private ScheduleDto AddDaysOffToSchedule(ScheduleDto schedule, List<DateOnly> daysOff)
    {
        if(daysOff.Count == 0) return schedule;
        var hourCount = schedule.Schedule.Count;
        foreach (var weekDay in daysOff.Select(DateHelper.GetWeekDay))
        {
            schedule.Schedule = schedule.Schedule.Select((weekDays, index) =>
            {
                var prop = weekDays.GetType().GetProperty(weekDay);
                if (index == 0)
                    prop.SetValue(weekDays, new ScheduleClass
                    {
                        Hours = hourCount,
                        Name = null,
                        Room = null,
                        Type = -1
                    });
                else
                    prop.SetValue(weekDays, null);
                return weekDays;
            }).ToList();
        }
        return schedule;
    }

    public async Task<ScheduleDto> GetSchedule(ScheduleInfoRequestDto scheduleInfoRequest)
    {
        var userClasses = _classesRepository.GetGroupsClasses(scheduleInfoRequest.fieldOfStudyLogId, scheduleInfoRequest.GroupIds).ToList();
        var hours = GetClassesHours(userClasses);
        var (startOfWeek, endOfWeek) = DateHelper.GetWeekStartAndEndDates(scheduleInfoRequest.Year, scheduleInfoRequest.WeekNumber);
        var thisWeekClasses = FilterClassesBetweenDates(userClasses, startOfWeek, endOfWeek);
        var organization = await _organizationRepository.GetOrganizationsInfo(scheduleInfoRequest.fieldOfStudyLogId);
        
        var daysOff = _organizationRepository.GetDaysOff(organization.Id).Result
            .Where(d => d >= startOfWeek && d <= endOfWeek).ToList();
        
        var schedule = new ScheduleDto
        {
            Date = $"{startOfWeek} - {endOfWeek}",
            CanFetchNextWeek =  DateHelper.GetDayDifference(organization.EndDay, startOfWeek.AddDays(7)) > 0,
            CanFetchPreviousWeek = DateHelper.GetDayDifference(organization.StartDay, endOfWeek.AddDays(-7)) < 0,
            Schedule = []
        };

        foreach (var t in hours)
        {
            var hour = ConvertDtos.ToHourDto(t);
            var scheduleWeekDays = new ScheduleWeekDays
            {
                Id = hour.HourId,
                Hour = $"{hour.StartTime} - {hour.EndTime}",
            };
            foreach (var thisWeekClass in thisWeekClasses)
            {
                if(thisWeekClass.classEntity.StartHourId != hour.HourId) continue;
                var weekDay = DateHelper.GetWeekDay(thisWeekClass.date);
                var prop = scheduleWeekDays.GetType().GetProperty(weekDay);
                var group = _groupRepository.GetGroupName(thisWeekClass.classEntity.Id);
                
                if(prop == null) throw new HttpNotFoundException($"Property '{weekDay}' not found in ScheduleWeekDays");
                prop.SetValue(scheduleWeekDays, new ScheduleClass
                {
                    Hours = thisWeekClass.classEntity.EndHourId - thisWeekClass.classEntity.StartHourId + 1,
                    Name = $"{thisWeekClass.classEntity.Name} ({group})",
                    Room = thisWeekClass.classEntity.Room,
                    Type = GetGroupType(thisWeekClass.classEntity.GroupId)
                });
            }
            schedule.Schedule.Add(scheduleWeekDays);
        }
        return AddDaysOffToSchedule(schedule, daysOff);
    }
}