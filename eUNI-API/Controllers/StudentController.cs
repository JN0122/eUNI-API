using System.ComponentModel.DataAnnotations;
using eUNI_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace eUNI_API.Controllers;

[Route("api/[controller]")]
public class StudentController(IStudentService studentService): ControllerBase
{
    private readonly IStudentService _studentService = studentService;
    
    [HttpGet("student-groups/")]
    public async Task<IActionResult> StudentGroups([FromQuery] [Required] Guid userId, [Required] int fieldOfStudyId)
    {
        var ids = await _studentService.GetStudentGroupIds(fieldOfStudyId, userId);
        return Ok(ids);
    }

    [HttpGet("student-fields-of-study")]
    public async Task<IActionResult> StudentFieldsOfStudies([FromQuery] [Required] Guid userId)
    {
        var fieldsOfStudy = await _studentService.GetStudentFieldsOfStudy(userId);
        return Ok(fieldsOfStudy);
    }
}