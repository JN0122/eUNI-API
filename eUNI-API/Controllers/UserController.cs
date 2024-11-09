using eUNI_API.Data;
using eUNI_API.Helpers;
using eUNI_API.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using eUNI_API.Models.Entities.Auth;
using eUNI_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace eUNI_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(AppDbContext context, IUserService userService): ControllerBase
{
    private readonly AppDbContext _context = context;
    private readonly IUserService _userService = userService;
    
    [Authorize]
    [HttpGet("user-info")]
    public async Task<ActionResult<User>> GetUser()
    {
        var user = await _userService.FindUserByClaim(User.Claims);
        
        return Ok(ConvertDtos.ToBasicUserDto(user));
    }

    [Authorize]
    [HttpPatch("change-password")]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
    {
        var user = await _userService.FindUserByClaim(User.Claims);
        if(!PasswordHasher.VerifyHashedPassword(changePasswordDto.OldPassword, user.Salt, user.PasswordHash))
            return BadRequest("Invalid old password");
        
        _userService.ChangePassword(user, changePasswordDto.NewPassword);
        return Ok();
    }
    
    [Authorize]
    [HttpPatch("change-email")]
    public async Task<ActionResult> ChangeEmail([FromBody] ChangeEmailDto changeEmailDto)
    {
        var user = await _userService.FindUserByClaim(User.Claims);
        
        _userService.ChangeEmail(user, changeEmailDto.Email);
        return Ok();
    }
}