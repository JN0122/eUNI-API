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
    private readonly string _calendarFolder = $"{env.WebRootPath}/calendars";
    
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

    public async Task WriteCalendarFileAsync(string filePath, Calendar calendar)
    {
        var directoryPath = Path.GetDirectoryName(filePath);
        var serializer = new CalendarSerializer();
        var calendarString = serializer.SerializeToString(calendar);
        
        if(!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath!);
        
        await File.WriteAllTextAsync(filePath, calendarString);
    }
    
    public string GetCalendarFilePath(FieldOfStudyInfoDto fieldOfStudyInfo, string groupName)
    {
        var studiesCycle = fieldOfStudyInfo.StudiesCycle == 1 ? "Inżynierskie" : 
            fieldOfStudyInfo.StudiesCycle == 2? "Magisterskie":throw new Exception("Not defined studies cycle");
        var typeOfStudies = fieldOfStudyInfo.IsFullTime ? "Stacjonarne" : "Niestacjonarne";
        return Path.Combine(_calendarFolder, fieldOfStudyInfo.YearName.Replace("/","-"), studiesCycle, fieldOfStudyInfo.Name, 
            typeOfStudies, fieldOfStudyInfo.Semester.ToString(), $"{groupName}.ics");
    }
}