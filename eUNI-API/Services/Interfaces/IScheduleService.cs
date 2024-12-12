using eUNI_API.Models.Dto.Classes;
using eUNI_API.Models.Dto.Schedule;
using eUNI_API.Models.Entities.OrganizationInfo;

namespace eUNI_API.Services.Interfaces;

public interface IScheduleService
{
    public Task<ScheduleDto> GetSchedule(ScheduleInfoRequestDto scheduleInfoRequest);
    public IEnumerable<HourDto> GetHours();
    public int GetGroupType(int groupId);
    public string GetGroupCalendarPath(int fieldOfStudyLogId, int groupId);
}