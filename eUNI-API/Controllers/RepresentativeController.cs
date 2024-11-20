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
public class RepresentativeController(IRepresentativeService representativeService, IUserService userService): ControllerBase
{
    private readonly IRepresentativeService _representativeService = representativeService;
    private readonly IUserService _userService = userService;
    
    [HttpGet("fields-of-study")]
    public async Task<IActionResult> GetFieldsOfStudyToEdit()
    {
        var user = await _userService.FindUserByClaim(User.Claims); 
        return Ok(await _representativeService.GetFieldOfStudyLogToEdit(user.Id));
    }

    [HttpGet("classes")]
    public async Task<IActionResult> GetClasses([FromQuery] [Required] int fieldOfStudyId)
    {
        var classes = await _representativeService.GetClasses(fieldOfStudyId);
        return Ok(classes);
    }
    
    [HttpPost("classes")]
    public async Task<IActionResult> CreateClass([FromBody] CreateClassRequestDto classRequestDto)
    {
        await _representativeService.CreateClass(classRequestDto);
        return Ok();
    }
    
    [HttpPut("classes/{id:int}")]
    public async Task<IActionResult> UpdateClass([FromBody] CreateClassRequestDto classRequestDto, [FromRoute] int id)
    {
        await _representativeService.UpdateClass(id, classRequestDto);
        return Ok();
    }
    
    [HttpDelete("classes/{id:int}")]
    public async Task<IActionResult> DeleteClass([FromRoute] [Required] int id)
    {
        await _representativeService.DeleteClass(id);
        return Ok();
    }
}