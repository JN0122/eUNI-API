using eUNI_API.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using eUNI_API.Data;
using eUNI_API.Helpers;
using eUNI_API.Models.Entities.User;
using eUNI_API.Services;
using eUNI_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

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
        
        if(!registrationDto.Password.Equals(registrationDto.ConfirmPassword))
            return BadRequest("Passwords do not match!");

        var salt = PasswordHasher.GenerateSalt();
        var userCreate = new CreateUser
        {
            Firstname = registrationDto.Firstname,
            Lastname = registrationDto.Lastname,
            Email = registrationDto.Email,
            PasswordHash = PasswordHasher.HashPassword(registrationDto.Password, salt),
            Salt = salt
        };

        var user = await _userService.CreateUser(userCreate);
        var token = _tokenService.CreateAccessToken(user);
        
        var response = new BasicUserDto
        {
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            Email = user.Email,
            Role = user.Role.Name
        };
        return Ok(response);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        User? user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

        if (user == null) {
            return Unauthorized("User not found!");
        }
    
        var isValidPassword = PasswordHasher.VerifyHashedPassword(loginDto.Password, user.Salt, user.PasswordHash);

        if (!isValidPassword) {
            return Unauthorized("Invalid credentials!");
        }
        
        var token = _tokenService.CreateAccessToken(user);
        
        Response.Cookies.Append("auth-token", token, new CookieOptions
        {
            HttpOnly = true,
            //Secure = true, //https
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddMinutes(30)
        });
        
        var response = new BasicUserDto
        {
            Firstname = user.Firstname, 
            Lastname = user.Lastname,
            Email = user.Email,
            Role = user.Role.Name
        };
        return Ok(response);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        Response.Cookies.Delete("auth-token");
        return Ok(new { message = "Logged out successfully" });
    }
        
    [HttpGet("validate-session")]
    [Authorize]
    public IActionResult ValidateSession()
    {
        return Ok(new { isAuthenticated = true });
    }
}