using eUNI_API.Helpers;
using eUNI_API.Models.Dto;
using eUNI_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eUNI_API.Controllers;

[Authorize(Roles = "SuperAdmin")]
[Route("api/[controller]")]
[ApiController]
public class UsersController(IUsersService usersService, IUserService userService): ControllerBase
{
    private readonly IUsersService _usersService = usersService;
    private readonly IUserService _userService = userService;
    
    [HttpGet("all-users")]
    public async Task<ActionResult<IEnumerable<UserInfoDto>>> GetUsers()
    {
        var users = await _usersService.GetUsers();
        
        return Ok(ConvertDtos.ToUserInfoDto(users));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteUser([FromRoute] Guid id)
    {
        var user = await _userService.FindUserByClaim(User.Claims);
        
        if (user == null)
            throw new UnauthorizedAccessException();

        if (user.Id.Equals(id))
            throw new ArgumentException("Cannot delete this user!");
        
        await _usersService.RemoveUser(id);
        return Ok();
    }

    [HttpPatch("{id:guid}")]
    public async Task<ActionResult> GetUserById([FromRoute] Guid id, [FromBody] UpdateUserDto updateUserDto)
    {
        var user = await _usersService.GetUserById(id);
        await _usersService.UpdateUser(user, updateUserDto);
        
        return Ok();
    }
}