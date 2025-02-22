using eUNI_API.Models.Dto.Auth;
using Microsoft.AspNetCore.Mvc;
using eUNI_API.Data;
using eUNI_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace eUNI_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(AppDbContext context, IUserService userService, ITokenService tokenService, IAuthService authService) : ControllerBase
{
    private readonly ITokenService _tokenService = tokenService;
    private readonly IAuthService _authService = authService;
    private readonly IUserService _userService = userService;
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
    {
        var user = await _authService.Register(registerRequest);
        
        var accessToken = _tokenService.CreateAccessToken(user.Id);
        var refreshToken = _tokenService.CreateRefreshToken(user.Id);
        
        _authService.AddRefreshToken(Response.Cookies, refreshToken);

        return Ok(new {
            AccessToken = accessToken
        });
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var user = await _authService.Login(loginDto);
        
        var oldRefreshToken = _authService.GetRefreshToken(Request.Cookies);
        _tokenService.RevokeRefreshToken(oldRefreshToken);
        
        var accessToken = _tokenService.CreateAccessToken(user.Id);
        var refreshToken = _tokenService.CreateRefreshToken(user.Id);
        
        _authService.AddRefreshToken(Response.Cookies, refreshToken);

        return Ok(new {
            AccessToken = accessToken
        });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var refreshToken = _authService.GetRefreshToken(Request.Cookies);
        _authService.RemoveRefreshToken(Response.Cookies);
        _tokenService.RevokeRefreshToken(refreshToken);
        return Ok();
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        var token = _authService.GetRefreshToken(Request.Cookies);

        if(token == null || !_tokenService.IsRefreshTokenValid(token))
            return Unauthorized();

        var userId = _tokenService.GetUserIdFromRefreshToken(token);
        
        var newRefreshToken = _tokenService.RegenerateRefreshToken(token);
        _authService.AddRefreshToken(Response.Cookies, newRefreshToken);
        
        var newAccessToken = _tokenService.CreateAccessToken(userId);
        
        return Ok(new {
            AccessToken = newAccessToken
        });
    }
}