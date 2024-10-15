using eUNI_API.Models.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { } 
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<RoleType> RoleTypes { get; set; }
    public DbSet<FieldOfStudy> FieldsOfStudy { get; set; }
    public DbSet<EmploymentUnit> EmploymentUnits { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RoleType>().HasData(
            new RoleType { Id = 1, Name = "SuperAdmin" },
            new RoleType { Id = 2, Name = "Admin" },
            new RoleType { Id = 3, Name = "Wykładowca" },
            new RoleType { Id = 4, Name = "Student" }
        );

        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, RoleTypeId = 1 }
        );
        
        modelBuilder.Entity<User>().HasData(
            new User { Id = new Guid("cc24a526-369e-46e0-a8d0-e179334ed456"), Firstname = "Jan", Lastname = "Kowalski", Email = "root@euni.com", RoleId = 1, PasswordHash = "f7f376a1fcd0d0e11a10ed1b6577c99784d3a6bbe669b1d13fae43eb64634f6e", Salt = "soleczka"}
        );
    }
}