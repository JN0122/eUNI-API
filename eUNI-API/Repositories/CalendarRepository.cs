using eUNI_API.Exception;
using eUNI_API.Models.Dto.Calendar;
using eUNI_API.Models.Dto.FieldOfStudy;
using eUNI_API.Repositories.Interfaces;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;

namespace eUNI_API.Repositories;

public class CalendarRepository(IWebHostEnvironment env): ICalendarRepository
{
    private const string CalendarFolder = "/calendars";

    public Calendar CreateGroupCalendar(List<EventDto> events)
    {
        var calendar = new Calendar
        {
            Method = "PUBLISH"
        };
        foreach (var eventDto in events)
        {
            foreach (var date in eventDto.Dates)
            {
                var startTime = eventDto.StartTime.Split(":");
                var startDt = new DateTime(date, new TimeOnly(int.Parse(startTime[0]), int.Parse(startTime[1])));
                
                var endTime = eventDto.EndTime.Split(":");
                var endDt = new DateTime(date, new TimeOnly(int.Parse(endTime[0]), int.Parse(endTime[1])));
                calendar.Events.Add(new CalendarEvent
                {
                    Summary = eventDto.ClassName,
                    Location = eventDto.ClassRoom,
                    DtStart = new CalDateTime(startDt),
                    DtEnd = new CalDateTime(endDt),
                });
            }
        }
        return calendar;
    }

    public async Task WriteCalendarFileAsync(string relativeFilePath, Calendar calendar)
    {
        var absoluteFilePath = env.WebRootPath + relativeFilePath;
        var absoluteDirectoryPath = Path.GetDirectoryName(absoluteFilePath);
        var serializer = new CalendarSerializer();
        var calendarString = serializer.SerializeToString(calendar);
        
        if(!Directory.Exists(absoluteDirectoryPath))
            Directory.CreateDirectory(absoluteDirectoryPath!);
        await File.WriteAllTextAsync(absoluteFilePath, calendarString);
    }

    private static List<string> SanitizePaths(params string[] paths)
    {
        return paths.Select(path => path.Replace('/', '-').Replace(' ', '-').ToLower()).ToList();
    }
    
    public string GetCalendarFilePath(FieldOfStudyInfoDto fieldOfStudyInfo, string groupName)
    {
        var studiesCycle = fieldOfStudyInfo.StudiesCycle == 1 ? "inz" : 
            fieldOfStudyInfo.StudiesCycle == 2? "mgr" : throw new HttpNotFoundException("Not defined studies cycle");
        var typeOfStudies = fieldOfStudyInfo.IsFullTime ? "Stacjonarne" : "Niestacjonarne";

        var sanitizedPaths = new List<string> { CalendarFolder };
        sanitizedPaths.AddRange(SanitizePaths(fieldOfStudyInfo.YearName, studiesCycle, fieldOfStudyInfo.Name,
            typeOfStudies, fieldOfStudyInfo.Semester.ToString()));
        sanitizedPaths.Add($"{groupName}.ics");
        
        return Path.Combine(sanitizedPaths.ToArray());
    }
}