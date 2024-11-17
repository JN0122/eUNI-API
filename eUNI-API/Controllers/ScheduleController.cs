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
}