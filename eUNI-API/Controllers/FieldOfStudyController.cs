using System.ComponentModel.DataAnnotations;
using eUNI_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eUNI_API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FieldOfStudyController(IFieldOfStudyService fieldOfStudyServices): ControllerBase
{
    private readonly IFieldOfStudyService _fieldOfStudyServices = fieldOfStudyServices;
    
    [HttpGet("groups")]
    public async Task<IActionResult> FieldsOfStudiesGroups([FromQuery] [Required] int fieldOfStudyLogId)
    {
        var groups = await _fieldOfStudyServices.GetGroups(fieldOfStudyLogId);
        return Ok(groups);
    }
    
    [HttpGet("logs")]
    public async Task<IActionResult> GetAllFieldsOfStudyLogs()
    {
        var groups = await _fieldOfStudyServices.GetFieldsOfStudyLogsInfoDtos();
        return Ok(groups);
    }
}