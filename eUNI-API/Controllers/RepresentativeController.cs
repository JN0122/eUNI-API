using eUNI_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eUNI_API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RepresentativeController(IRepresentativeService representativeService, IUserService userService): ControllerBase
{
    private readonly IRepresentativeService _representativeService = representativeService;
    private readonly IUserService _userService = userService;
    
    [HttpPost("get-fields-of-study-ids-to-edit")]
    public async Task<IActionResult> GetFieldsOfStudyIdsToEdit()
    {
        var isRepresentative = User.FindFirst("IsRepresentative")?.Value;
        if ((isRepresentative == null || !bool.Parse(isRepresentative)) && !User.IsInRole("Admin")) return Unauthorized();
        
        var user = await _userService.FindUserByClaim(User.Claims);
        return Ok(await _representativeService.GetFieldOfStudyLogIdsToEdit(user.Id));
    }
}