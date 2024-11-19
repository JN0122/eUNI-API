using eUNI_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eUNI_API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RepresentativeController(IRepresentativeService representativeService, IUserService userService, IAuthService authService): ControllerBase
{
    private readonly IRepresentativeService _representativeService = representativeService;
    private readonly IUserService _userService = userService;
    private readonly IAuthService _authService = authService;
    
    [HttpGet("get-fields-of-study-to-edit")]
    public async Task<IActionResult> GetFieldsOfStudyToEdit()
    {
        if(!_authService.IsRepresentative(User.Claims))
            return Unauthorized();
        var user = await _userService.FindUserByClaim(User.Claims); 
        return Ok(await _representativeService.GetFieldOfStudyLogToEdit(user.Id));
    }
}