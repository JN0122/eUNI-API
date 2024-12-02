using eUNI_API.Models.Dto.Calendar;
using eUNI_API.Models.Dto.FieldOfStudy;
using Ical.Net;

namespace eUNI_API.Repositories.Interfaces;

public interface ICalendarRepository
{
    public Calendar CreateGroupCalendar(List<EventDto> events);
    public Task WriteCalendarFileAsync(string filePath, Calendar calendar);
    public string GetCalendarFilePath(FieldOfStudyInfoDto fieldOfStudyInfo, string groupName);
}