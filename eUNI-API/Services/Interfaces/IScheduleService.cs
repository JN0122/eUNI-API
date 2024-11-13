using eUNI_API.Models.Dto;
using eUNI_API.Models.Dto.Schedule;

namespace eUNI_API.Services.Interfaces;

public interface IScheduleService
{
    public Task<ScheduleDto> GetSchedule(ScheduleInfoDto scheduleInfo);
}