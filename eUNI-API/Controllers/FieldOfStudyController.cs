using System.ComponentModel.DataAnnotations;
using eUNI_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eUNI_API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FieldOfStudyController(IStudentService studentService): ControllerBase
{
    private readonly IStudentService _studentService = studentService;
    
    [HttpGet("groups")]
    public async Task<IActionResult> StudentFieldsOfStudies([FromQuery] [Required] int fieldOfStudyLogId)
    {
        var groups = await _studentService.GetGroups(fieldOfStudyLogId);
        return Ok(groups);
    }
}