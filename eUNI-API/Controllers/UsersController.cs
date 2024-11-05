using eUNI_API.Helpers;
using eUNI_API.Models.Dto;
using eUNI_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eUNI_API.Controllers;

[Authorize(Roles = "SuperAdmin")]
[Route("api/[controller]")]
[ApiController]
public class UsersController(IUsersService usersService): ControllerBase
{
    private readonly IUsersService _usersService = usersService;
    
    [HttpGet("all-users")]
    public async Task<ActionResult<IEnumerable<UserInfoDto>>> GetUsers()
    {
        var users = await _usersService.GetUsers();
        
        return Ok(ConvertDtos.ToUserInfoDto(users));
    }
}