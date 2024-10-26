using System.Security.Claims;
using eUNI_API.Data;
using eUNI_API.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eUNI_API.Models.Entities.User;
using eUNI_API.Services;
using Microsoft.AspNetCore.Authorization;

namespace eUNI_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(AppDbContext context, IUserService userService): ControllerBase
{
    private readonly AppDbContext _context = context;
    private readonly IUserService _userService = userService;
    
    [HttpGet]
    [Route("Lecturers")]
    public async Task<ActionResult<IEnumerable<Lecturer>>> GetLecturers()
    {
        var lecturers = await _context.Users
            .Include(u=>u.Lecturer)
            .ThenInclude(l=>l.EmploymentUnit)
            .ThenInclude(e => e.AcademicDepartment)
            .Where(u=>u.Lecturer != null)
            .Select(u => new Lecturer
            {
                Id = u.Id,
                Firstname = u.Firstname,
                Lastname = u.Lastname,
                Email = u.Email,
                RoleName = u.Role.Name,
                EmploymentUnit = u.Lecturer.EmploymentUnit.Abbr,
                AcademicDepartment = u.Lecturer.EmploymentUnit.AcademicDepartment.Abbr
            }).ToListAsync();
        return lecturers;
    }

    [HttpGet("getuser")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<ActionResult<User>> GetUser()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        
        if(userIdClaim == null)
            return Unauthorized("No user ID claim present in token.");
        
        try
        {
            var user = await _userService.FindUserByClaimId(userIdClaim);
            return Ok(user);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}