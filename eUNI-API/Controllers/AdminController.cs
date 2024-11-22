using eUNI_API.Helpers;
using eUNI_API.Models.Dto;
using eUNI_API.Models.Dto.Organization;
using eUNI_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eUNI_API.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
[ApiController]
public class AdminController(IAdminService adminService, IUserService userService, IOrganizationService organizationService): ControllerBase
{
    private readonly IAdminService _adminService = adminService;
    private readonly IUserService _userService = userService;
    private readonly IOrganizationService _organizationService = organizationService;
    
    [HttpGet("users")]
    public async Task<ActionResult<IEnumerable<UserInfoDto>>> GetUsers()
    {
        var users = await _adminService.GetUsers();
        
        return Ok(ConvertDtos.ToUserInfoDto(users));
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

    [HttpPatch("users/{id:guid}")]
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
    
    [HttpPost("years")]
    public async Task<ActionResult> CreateYear([FromBody] YearRequest yearRequest)
    {
        await _organizationService.CreateYear(yearRequest);
        return Ok();
    }
    
    [HttpPut("years/{id:int}")]
    public async Task<ActionResult> UpdateYear([FromBody] YearRequest yearRequest, [FromRoute] int id)
    {
        await _organizationService.UpdateYear(id, yearRequest);
        return Ok();
    }

    [HttpDelete("years/{id:int}")]
    public async Task<ActionResult> DeleteYear([FromRoute] int id)
    {
        await _organizationService.DeleteYear(id);
        return Ok();
    }
}