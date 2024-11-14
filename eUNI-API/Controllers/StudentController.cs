using eUNI_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace eUNI_API.Controllers;

[Route("api/[controller]")]
public class StudentController(IStudentService studentService): ControllerBase
{
    private readonly IStudentService _studentService = studentService;
    
    [HttpGet("student-groups/")]
    public async Task<IActionResult> StudentGroups([FromQuery] Guid userId, int fieldOfStudyId)
    {
        var ids = await _studentService.GetStudentGroupIds(fieldOfStudyId, userId);
        return Ok(ids);
    }

    [HttpGet("student-field-of-study")]
    public async Task<IActionResult> StudentFieldOfStudies([FromQuery] Guid userId)
    {
        var fieldsOfStudy = await _studentService.GetStudentFieldsOfStudy(userId);
        return Ok(fieldsOfStudy);
    }
}