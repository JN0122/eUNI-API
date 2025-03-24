using eUNI_API.Models.Dto.Setup;
using eUNI_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace eUNI_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SetupController(ISetupService setupService) : ControllerBase
{
    private readonly ISetupService _setupService = setupService;
    
    [HttpPost("set-password")]
    public async Task<IActionResult> SetRootPassword([FromBody] RootDto rootDto)
    {
        await _setupService.ResetRootAccount(rootDto.Password);
        return Ok("Root password set!");
    }
}