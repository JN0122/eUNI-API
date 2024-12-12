using eUNI_API.Models.Dto;
using eUNI_API.Models.Dto.FieldOfStudy;
using eUNI_API.Models.Dto.Organization;
using eUNI_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eUNI_API.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
[ApiController]
public class AdminController(IAdminService adminService, IUserService userService, 
    IOrganizationService organizationService, IRepresentativeService representativeService,
    IFieldOfStudyService fieldOfStudyService): ControllerBase
{
    private readonly IAdminService _adminService = adminService;
    private readonly IUserService _userService = userService;
    private readonly IOrganizationService _organizationService = organizationService;
    private readonly IRepresentativeService _representativeService = representativeService;
    private readonly IFieldOfStudyService _fieldOfStudyService = fieldOfStudyService;
    
    [HttpGet("users")]
    public async Task<ActionResult<IEnumerable<UserInfoDto>>> GetUsers()
    {
        var users = await _adminService.GetUsers();
        var usersInfo = _representativeService.GetUsersInfoDto(users);
        return Ok(usersInfo);
    }

    [HttpDelete("users/{id:guid}")]
    public async Task<ActionResult> DeleteUser([FromRoute] Guid id)
    {
        var user = await _userService.FindUserByClaim(User.Claims);
        
        if (user == null)
            throw new UnauthorizedAccessException();

        if (user.Id.Equals(id))
            throw new ArgumentException("Cannot delete this user!");
        
        await _adminService.RemoveUser(id);
        return Ok();
    }
    
    [HttpPost("users")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDto createUserRequestDto)
    {
        await _adminService.CreateUser(createUserRequestDto);
        return Ok();
    }

    [HttpPut("users/{id:guid}")]
    public async Task<ActionResult> GetUserById([FromRoute] Guid id, [FromBody] UpdateUserRequestDto updateUserRequestDto)
    {
        await _adminService.UpdateUser(id, updateUserRequestDto);
        return Ok();
    }
    
    [HttpGet("year-organizations")]
    public async Task<ActionResult<IEnumerable<YearOrganization>>> GetYearOrganizations()
    {
        var organizations = await _organizationService.GetYearOrganizations();
        return Ok(organizations);
    }
    
    [HttpPost("year-organizations")]
    public async Task<ActionResult> CreateYearOrganization(
        [FromBody] YearOrganizationRequest yearOrganizationRequest)
    {
        await _organizationService.CreateYearOrganization(yearOrganizationRequest);
        return Ok();
    }
    
    [HttpPut("year-organizations/{id:int}")]
    public async Task<ActionResult> UpdateYearOrganization(
        [FromBody] YearOrganizationRequest yearOrganizationRequest,
        [FromRoute] int id)
    {
        await _organizationService.UpdateYearOrganization(id, yearOrganizationRequest);
        return Ok();
    }

    [HttpDelete("year-organizations/{id:int}")]
    public async Task<ActionResult> DeleteYearOrganization([FromRoute] int id)
    {
        await _organizationService.DeleteYearOrganization(id);
        return Ok();
    }
    
    [HttpGet("years")]
    public async Task<ActionResult<IEnumerable<YearDto>>> GetYears()
    {
        var years = await _organizationService.GetYears();
        return Ok(years);
    }
    
    [HttpGet("next-semester-details")]
    public async Task<ActionResult<IEnumerable<YearDto>>> GetNextSemesterDetails()
    {
        var details = await _organizationService.GetNextSemesterDetails();
        return Ok(details);
    }

    [HttpGet("available-fields")]
    public async Task<ActionResult<IEnumerable<FieldOfStudyDto>>> GetAvailableFields()
    {
        var fields = await _fieldOfStudyService.GetFieldsOfStudy();
        return Ok(fields);
    }
    
    [HttpPost("available-fields")]
    public async Task<ActionResult> CreateAvailableField(
        [FromBody] CreateFieldOfStudyRequest createFieldOfStudyRequest)
    {
        await _fieldOfStudyService.CreateFieldOfStudy(createFieldOfStudyRequest);
        return Ok();
    }
    
    [HttpPut("available-fields/{id:int}")]
    public async Task<ActionResult> UpdateAvailableField(
        [FromBody] CreateFieldOfStudyRequest createFieldOfStudyRequest,
        [FromRoute] int id)
    {
        await _fieldOfStudyService.UpdateFieldOfStudy(id, createFieldOfStudyRequest);
        return Ok();
    }

    [HttpDelete("available-fields/{id:int}")]
    public async Task<ActionResult> DeleteAvailableField([FromRoute] int id)
    {
        await _fieldOfStudyService.DeleteFieldOfStudy(id);
        return Ok();
    }
    
    [HttpGet("field-of-study-requirements")]
    public async Task<ActionResult<AcademicYearDetails>> GetFieldOfStudyRequirements()
    {
        var requirements = await _organizationService.GetSemesterDetailsToUpgrade();
        return Ok(requirements);
    }
}