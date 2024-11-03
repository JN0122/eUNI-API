using System.Security.Claims;
using eUNI_API.Data;
using eUNI_API.Helpers;
using eUNI_API.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eUNI_API.Models.Entities.User;
using eUNI_API.Services;
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
}