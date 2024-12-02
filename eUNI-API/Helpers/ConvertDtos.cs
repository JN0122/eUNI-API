using eUNI_API.Models.Dto.Calendar;
using eUNI_API.Models.Dto.Classes;
using eUNI_API.Models.Dto.FieldOfStudy;
using eUNI_API.Models.Entities.FieldOfStudy;
using eUNI_API.Models.Entities.OrganizationInfo;

namespace eUNI_API.Helpers;

public static class ConvertDtos
{
    public static FieldOfStudyInfoDto ToFieldOfStudyInfoDto(FieldOfStudyLog fieldOfStudyLog)
    {
        return new FieldOfStudyInfoDto
        {
            FieldOfStudyLogId = fieldOfStudyLog.Id,
            Name = fieldOfStudyLog.FieldOfStudy.Name,
            Semester = fieldOfStudyLog.Semester,
            StudiesCycle = fieldOfStudyLog.FieldOfStudy.StudiesCycle,
            IsFullTime = fieldOfStudyLog.FieldOfStudy.IsFullTime,
            YearName = fieldOfStudyLog.OrganizationsOfTheYear.Year.Name
        };
    }

    public static HourDto ToHourDto(Hour hour)
    {
        var formatedStartMinute = (hour.StartMinute < 10 ? "0" : "") + hour.StartMinute;
        var formateEndMinute = (hour.EndMinute < 10 ? "0" : "") + hour.EndMinute;
        return new HourDto
        {
            HourId = hour.Id,
            StartTime = $"{hour.StartHour}:{formatedStartMinute}",
            EndTime = $"{hour.EndHour}:{formateEndMinute}"
        };
    }

    public static EventDto ToEventDto(ClassDto classDto)
    {
        return new EventDto
        {
            ClassName = ClassHelper.GetClassWithGroup(classDto.ClassName, classDto.GroupName),
            ClassRoom = classDto.ClassRoom,
            Dates = classDto.Dates.ToList(),
            StartTime = classDto.StartHour.StartTime,
            EndTime = classDto.EndHour.EndTime
        };
    }
    
    public static List<EventDto> ToEventDto(List<ClassDto> classes)
    {
        return classes.Select(ToEventDto).ToList();
    }
}