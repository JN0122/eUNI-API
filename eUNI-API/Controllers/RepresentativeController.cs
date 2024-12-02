using System.ComponentModel.DataAnnotations;
using eUNI_API.Attributes;
using eUNI_API.Models.Dto.Classes;
using eUNI_API.Models.Entities.FieldOfStudy;
using eUNI_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eUNI_API.Controllers;

[Authorize]
[RepresentativeOnly]
[ApiController]
[Route("api/[controller]")]
public class RepresentativeController(IRepresentativeService representativeService, IUserService userService, 
    IOrganizationService organizationService): ControllerBase
{
    private readonly IRepresentativeService _representativeService = representativeService;
    private readonly IUserService _userService = userService;
    private readonly IOrganizationService _organizationService = organizationService;
    
    [HttpGet("fields-of-study")]
    public async Task<IActionResult> GetFieldsOfStudyToEdit()
    {
        var user = await _userService.FindUserByClaim(User.Claims); 
        return Ok(await _representativeService.FieldOfStudyLogsToEdit(user.Id));
    }
    
    [HttpGet("academic-year-days-off")]
    public async Task<IActionResult> GetDaysOff([FromQuery] [Required] int fieldOfStudyLogId)
    {
        var academicYearDaysOff = _organizationService.GetAcademicYearDaysOff(fieldOfStudyLogId); 
        return Ok(academicYearDaysOff);
    }

    [HttpGet("classes")]
    public async Task<IActionResult> GetClasses([FromQuery] [Required] int fieldOfStudyLogId)
    {
        var classes = await _representativeService.GetClasses(fieldOfStudyLogId);
        return Ok(classes);
    }
    
    [HttpPost("classes")]
    public async Task<IActionResult> CreateClass([FromBody] CreateClassRequestDto classRequestDto)
    {
        await _representativeService.CreateClass(classRequestDto);
        return Ok();
    }
    
    [HttpPut("classes/{classId:int}")]
    public async Task<IActionResult> UpdateClass([FromBody] UpdateClassRequestDto updateClassRequestDto, [FromRoute] int classId)
    {
        await _representativeService.UpdateClass(classId, updateClassRequestDto);
        return Ok();
    }
    
    [HttpDelete("classes/{id:int}")]
    public async Task<IActionResult> DeleteClass([FromRoute] [Required] int id)
    {
        await _representativeService.DeleteClass(id);
        return Ok();
    }
    
    [HttpGet("assignments")]
    public async Task<IActionResult> GetAssignments([FromQuery] [Required] int fieldOfStudyLogId)
    {
        var assignments = await _representativeService.GetAssignments(fieldOfStudyLogId);
        return Ok(assignments);
    }
    
    [HttpPost("assignments")]
    public async Task<IActionResult> CreateAssigment([FromBody] CreateAssignmentRequestDto assignmentRequestDto)
    {
        await _representativeService.CreateAssignment(assignmentRequestDto);
        return Ok();
    }
    
    [HttpPut("assignments/{id:int}")]
    public async Task<IActionResult> UpdateAssigment([FromRoute] [Required] int id, [FromBody] [Required] CreateAssignmentRequestDto assignmentRequestDto)
    {
        await _representativeService.UpdateAssignment(id, assignmentRequestDto);
        return Ok();
    }
    
    [HttpDelete("assignments/{id:int}")]
    public async Task<IActionResult> DeleteAssignment([FromRoute] [Required] int id)
    {
        await _representativeService.DeleteAssignment(id);
        return Ok();
    }
    
    [HttpGet("all-groups")]
    public async Task<IActionResult> GetAllGroups([Required] int fieldOfStudyLogId)
    {
        var groups = _representativeService.GetAllGroups(fieldOfStudyLogId);
        return Ok(groups);
    }
}