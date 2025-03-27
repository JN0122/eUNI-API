using eUNI_API.Models.Dto.Setup;
using eUNI_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace eUNI_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SetupController(ISetupService setupService, IHostEnvironment env) : ControllerBase
{
    private readonly ISetupService _setupService = setupService;
    private readonly IHostEnvironment _env = env;
    
    [HttpPost("set-password")]
    public async Task<IActionResult> SetRootPassword([FromBody] RootDto rootDto)
    {
        if (_env.IsProduction()) return Forbid();
        await _setupService.ResetRootAccount(rootDto.Password);
        return Ok();
    }

    [HttpPost("reset-db")]
    public IActionResult ResetDatabase()
    {
        if (_env.IsProduction()) return Forbid();
        _setupService.ResetDb();
        return Ok();
    }
}