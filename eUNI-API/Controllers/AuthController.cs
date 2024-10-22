using System.Security.Claims;
using eUNI_API.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using eUNI_API.Data;
using eUNI_API.Helpers;
using eUNI_API.Models.Dto.Auth;
using eUNI_API.Models.Entities.User;
using eUNI_API.Services;
using Microsoft.AspNetCore.Authorization;

namespace eUNI_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(AppDbContext context, IUserService userService, ITokenService tokenService) : ControllerBase
{
    private readonly AppDbContext _context = context;
    private readonly IUserService _userService = userService;
    private readonly ITokenService _tokenService = tokenService;
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationRequest request)
    {
        // TODO check if the user has proper permissions
        
        if(!request.Password.Equals(request.ConfirmPassword))
            return BadRequest("Passwords do not match!");

        var salt = PasswordHasher.GenerateSalt();
        var userCreate = new UserCreate
        {
            Firstname = request.Firstname,
            Lastname = request.Lastname,
            Email = request.Email,
            PasswordHash = PasswordHasher.HashPassword(request.Password, salt),
            Salt = salt
        };

        var user = await _userService.CreateUser(userCreate);
        var token = _tokenService.CreateToken(user);
        var response = new AuthResponse
        {
            UserId = user.Id,
            Email = user.Email,
            Role = user.Role.Name,
            Token = token
        };
        return Ok(response);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        User? user = await _userService.FindByEmailAsync(request.Email);        

        if (user == null) {
            return Unauthorized("Invalid credentials!");
        }
    
        var isValidPassword = PasswordHasher.VerifyHashedPassword(request.Password, user.Salt, user.PasswordHash);

        if (!isValidPassword) {
            return Unauthorized("Invalid credentials!");
        }
        
        var token = _tokenService.CreateToken(user);
        var response = new AuthResponse
        {
            UserId = user.Id,
            Email = user.Email,
            Role = user.Role.Name,
            Token = token
        };
        return Ok(response);
    }
    
    [HttpGet("getuser")]
    [Authorize(Roles = "USER")]
    public async Task<ActionResult<User>> GetUser()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        
        Console.WriteLine("Claims received:");
        foreach (var claim in User.Claims)
        {
            Console.WriteLine($"{claim.Type}: {claim.Value}");
        }

        if(userIdClaim == null)
        {
            return Unauthorized("No user ID claim present in token.");
        }
        
        try
        {
            var user = await _userService.FindByEmailAsync(userIdClaim);
            return Ok(user);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}