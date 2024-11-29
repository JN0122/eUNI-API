using eUNI_API.Data;
using eUNI_API.Helpers;
using eUNI_API.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using eUNI_API.Models.Entities.Auth;
using eUNI_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace eUNI_API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService userService, IRepresentativeService representativeService): ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly IRepresentativeService _representativeService = representativeService;
    
    [HttpGet("info")]
    public async Task<ActionResult<User>> GetUser()
    {
        var user = await _userService.FindUserByClaim(User.Claims);
        return Ok(_representativeService.GetUserInfoDto(user));
    }

    [HttpPatch("password")]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordRequestDto changePasswordRequestDto)
    {
        var user = await _userService.FindUserByClaim(User.Claims);
        if(!PasswordHasher.VerifyHashedPassword(changePasswordRequestDto.OldPassword, user.Salt, user.PasswordHash))
            return BadRequest("Invalid old password");
        
        _userService.ChangePassword(user, changePasswordRequestDto.NewPassword);
        return Ok();
    }
    
    [HttpPatch("email")]
    public async Task<ActionResult> ChangeEmail([FromBody] ChangeEmailRequestDto changeEmailRequestDto)
    {
        var user = await _userService.FindUserByClaim(User.Claims);
        
        _userService.ChangeEmail(user, changeEmailRequestDto.Email);
        return Ok();
    }
}