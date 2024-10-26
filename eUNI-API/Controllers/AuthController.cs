using eUNI_API.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using eUNI_API.Data;
using eUNI_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace eUNI_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(AppDbContext context, IUserService userService, ITokenService tokenService, IAuthService authService) : ControllerBase
{
    private readonly AppDbContext _context = context;
    private readonly IUserService _userService = userService;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IAuthService _authService = authService;
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationDto registrationDto)
    {
        // TODO check if the user has proper permissions
        try
        {
            var user = await _authService.Register(registrationDto);
            var token = _tokenService.CreateAccessToken(user);
        
            var response = new BasicUserDto
            {
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
                AuthToken = token
            };
            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            var user = await _authService.Login(loginDto);
            var accessToken = _tokenService.CreateAccessToken(user);
            
            _authService.AddRefreshToken(Response.Cookies, accessToken);
            
            var response = new BasicUserDto
            {
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
                AuthToken = accessToken
            };
            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        _authService.RemoveRefreshToken(Response.Cookies);
        return Ok();
    }
        
    [HttpGet("validate-session")]
    [Authorize]
    public IActionResult ValidateSession()
    {
        return Ok(new { isAuthenticated = true });
    }
}