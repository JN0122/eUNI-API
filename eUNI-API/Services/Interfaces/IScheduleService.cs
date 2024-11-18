using eUNI_API.Models.Dto.Schedule;
using eUNI_API.Models.Entities.OrganizationInfo;

namespace eUNI_API.Services.Interfaces;

public interface IScheduleService
{
    public Task<ScheduleDto> GetSchedule(ScheduleInfoRequestDto scheduleInfoRequest);
    public List<Hour> GetHours();
    public ClassAssignment? GetClassAssigment(int classId, DateOnly date);
    public int GetGroupType(int groupId);
    // classes, assignment logic
}