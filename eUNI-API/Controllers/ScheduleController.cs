using System.ComponentModel.DataAnnotations;
using eUNI_API.Models.Dto.Schedule;
using eUNI_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eUNI_API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ScheduleController(IScheduleService scheduleService): ControllerBase
{
    private readonly IScheduleService _scheduleService = scheduleService;

    [HttpPost("get-schedule")]
    public async Task<IActionResult> GetSchedule([FromBody] ScheduleInfoRequestDto scheduleInfoRequest)
    {
        var schedule = await _scheduleService.GetSchedule(scheduleInfoRequest);
        return Ok(schedule);
    }
    
    [HttpGet("hours")]
    public async Task<IActionResult> GetHours()
    {
        return Ok(_scheduleService.GetHours());
    }

    [HttpGet("group-calendar-path")]
    public async Task<IActionResult> GetGroupCalendarPath([FromQuery, Required] int fieldOfStudyLogId, [FromQuery, Required] int groupId)
    {
        return Ok(_scheduleService.GetGroupCalendarPath(fieldOfStudyLogId, groupId));
    }
}