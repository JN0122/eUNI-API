using System.ComponentModel.DataAnnotations;
using eUNI_API.Models.Dto.FieldOfStudy;
using eUNI_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eUNI_API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class StudentController(IStudentService studentService, IUserService userService): ControllerBase
{
    private readonly IStudentService _studentService = studentService;
    private readonly IUserService _userService = userService;

    [HttpGet("info")]
    public async Task<IActionResult> StudentFieldsOfStudies()
    {
        var user = await _userService.FindUserByClaim(User.Claims); 
        var studentInfo = await _studentService.GetStudentInfo(user.Id);
        return Ok(studentInfo);
    }
}