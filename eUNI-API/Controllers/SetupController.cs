using eUNI_API.Models.Dto.Setup;
using Microsoft.AspNetCore.Mvc;

namespace eUNI_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SetupController : ControllerBase
{
    [HttpPost("set-password")]
    public async Task<IActionResult> SetRootPassword([FromBody] RootDto rootDto)
    {
        return Ok("Seeded! Root password = " + rootDto.Password);
    }
}