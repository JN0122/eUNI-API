using eUNI_API.Data;
using eUNI_API.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eUNI_API.Models.Entities.User;

namespace eUNI_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserInfo>>> GetUsers()
    {
        var user = await _context.Users
            .Include(u => u.Role)
            .ThenInclude(r => r.RoleType) 
            .Select(u => new UserInfo
            {
                Id = u.Id,
                Firstname = u.Firstname,
                Lastname = u.Lastname,
                Email = u.Email,
                RoleName = u.Role.RoleType.Name,
            }).ToListAsync();
        return user;
    }
    
    [HttpPost]
    public async Task<IResult> CreateStudent(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
        
        return Results.Ok();
    }
}