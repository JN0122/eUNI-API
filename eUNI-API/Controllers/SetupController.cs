using eUNI_API.Enums;
using eUNI_API.Models.Dto.Classes;
using eUNI_API.Models.Dto.FieldOfStudy;
using eUNI_API.Models.Dto.Setup;
using eUNI_API.Models.Dto.Student;
using eUNI_API.Models.Dto.User;
using eUNI_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace eUNI_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SetupController(ISetupService setupService, IStudentService studentService, 
    IFieldOfStudyService fieldOfStudyService, IOrganizationService organizationService,
    IAdminService adminService, IRepresentativeService representativeService,
    IHostEnvironment env) : ControllerBase
{
    private readonly ISetupService _setupService = setupService;
    private readonly IHostEnvironment _env = env;
    private readonly IStudentService _studentService = studentService;
    private readonly IFieldOfStudyService _fieldOfStudyService = fieldOfStudyService;
    private readonly IOrganizationService _organizationService = organizationService;
    private readonly IAdminService _adminService = adminService;
    private readonly IRepresentativeService _representativeService = representativeService;
    
    [HttpPost("set-password")]
    public async Task<IActionResult> SetRootPassword([FromBody] UserDto userDto)
    {
        if (_env.IsProduction()) return Forbid();
        await _setupService.ResetRootAccount(userDto.Password);
        return Ok();
    }

    [HttpPost("reset-db")]
    public IActionResult ResetDatabase()
    {
        if (_env.IsProduction()) return Forbid();
        _setupService.ResetDb();
        return Ok();
    }
    
    [HttpPost("seed-db")]
    public async Task<IActionResult> SeedDatabase([FromBody] UserDto userDto)
    {
        if (_env.IsProduction()) return Forbid();
        var fieldOfStudy = await _fieldOfStudyService.CreateFieldOfStudy(new CreateFieldOfStudyRequest
        {
            Abbr = "K",
            FullTime = true,
            Name = "Informatyka Stosowana",
            SemesterCount = 7,
            StudiesCycle = 1
        });
        
        var organizations = await _organizationService.GetYearOrganizations();
        var newestOrganizationId = organizations.Max(o => o.Id);
        
        var fieldOfStudyLog = await _fieldOfStudyService.CreateFieldOfStudyLog(new CreateFieldOfStudyLogRequest
        {
            CurrentSemester = 1,
            FieldOfStudyId = fieldOfStudy.Id,
            OrganizationId = newestOrganizationId
        });
        
        var user = await _adminService.CreateUser(new CreateUserRequestDto
        {
            Firstname = "Adam",
            Lastname = "Nowak",
            Email = "adam@euni.com",
            Password = userDto.Password,
            RoleId = (int)UserRole.Student,
            RepresentativeFieldsOfStudyLogIds = [fieldOfStudyLog.Id]
        });

        await _studentService.SetCurrentFieldOfStudy(user.Id, fieldOfStudy.Id);
        
        await _representativeService.CreateClass(new CreateClassRequestDto
        {
            FieldOfStudyLogId = fieldOfStudyLog.Id,
            Name = "Informatyka",
            Room = "A123",
            IsOddWeek = null,
            WeekDay = WeekDay.Monday,
            GroupId = 6,
            StartHourId = 3,
            EndHourId = 3
        });
        
        await _representativeService.CreateClass(new CreateClassRequestDto
        {
            FieldOfStudyLogId = fieldOfStudyLog.Id,
            Name = "Matematyka",
            Room = "G118",
            IsOddWeek = null,
            WeekDay = WeekDay.Tuesday,
            GroupId = 7,
            StartHourId = 2,
            EndHourId = 4
        });
        
        await _studentService.ChangeStudentGroup(user.Id, new StudentChangeGroupRequestDto
        {
            FieldOfStudyLogId = fieldOfStudyLog.Id,
            GroupId = 6,
            GroupType = 0
        });
        
        await _studentService.ChangeStudentGroup(user.Id, new StudentChangeGroupRequestDto
        {
            FieldOfStudyLogId = fieldOfStudyLog.Id,
            GroupId = 7,
            GroupType = 3
        });
        
        return Ok();
    }
}