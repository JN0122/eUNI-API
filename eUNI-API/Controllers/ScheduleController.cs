using eUNI_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eUNI_API.Controllers;

//[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ScheduleController(IScheduleService scheduleService): ControllerBase
{
    private readonly IScheduleService _scheduleService = scheduleService;
    
    [HttpPost("calculate-classes-dates")]
    public async Task<IActionResult> CalculateClassesDates(int classId)
    {
        await _scheduleService.CalculateClassesDates(classId);
        return Ok();
    }
}