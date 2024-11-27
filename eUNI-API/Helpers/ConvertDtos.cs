using eUNI_API.Models.Dto;
using eUNI_API.Models.Dto.FieldOfStudy;
using eUNI_API.Models.Entities.Auth;
using eUNI_API.Models.Entities.FieldOfStudy;

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
            IsFullTime = fieldOfStudyLog.FieldOfStudy.IsFullTime
        };
    }
}