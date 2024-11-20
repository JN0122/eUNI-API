using eUNI_API.Attributes;
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
    
    [HttpGet("get-fields-of-study-to-edit")]
    public async Task<IActionResult> GetFieldsOfStudyToEdit()
    {
        var user = await _userService.FindUserByClaim(User.Claims); 
        return Ok(await _representativeService.GetFieldOfStudyLogToEdit(user.Id));
    }
}