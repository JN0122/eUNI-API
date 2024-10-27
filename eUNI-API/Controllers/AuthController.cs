using eUNI_API.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using eUNI_API.Data;
using eUNI_API.Helpers;
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
            var token = _tokenService.CreateAccessToken(user.Id);
            
            return Ok(ConvertDtos.ToBasicUserDto(user, token));
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
            
            _tokenService.RevokeUserToken(user.Id);
            
            var accessToken = _tokenService.CreateAccessToken(user.Id);
            var refreshToken = _tokenService.CreateRefreshToken(user.Id);
            
            _authService.AddRefreshToken(Response.Cookies, refreshToken);

            return Ok(ConvertDtos.ToBasicUserDto(user, accessToken));
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

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        var token = _authService.GetRefreshToken(Request.Cookies);

        if(token == null || !_tokenService.IsRefreshTokenValid(token))
            return Unauthorized();

        var user = _tokenService.GetUserByRefreshToken(token);
        
        _tokenService.DeleteRefreshToken(token);
        var newRefreshToken = _tokenService.CreateRefreshToken(user.Id);
        _authService.AddRefreshToken(Response.Cookies, newRefreshToken);
        
        var newAccessToken = _tokenService.CreateAccessToken(user.Id);
        
        return Ok(new AccessTokenDto
        {
            AccessToken = newAccessToken
        });
    }
        
    [HttpGet("validate-session")]
    [Authorize]
    public IActionResult ValidateSession()
    {
        return Ok(new { isAuthenticated = true });
    }
}