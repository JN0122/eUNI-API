using eUNI_API.Enums;
using eUNI_API.Models.Entities.AcademicInfo;
using eUNI_API.Models.Entities.User;
using eUNI_API.Models.Entities.UserInfo;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { } 
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<PasswordResetLog> PasswordResetLogs { get; set; }
    public DbSet<Lecturer> Lecturers { get; set; }
    public DbSet<EmploymentUnit> EmploymentUnits { get; set; }
    public DbSet<AcademicDepartment> AcademicDepartments { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = (int)UserRole.SuperAdmin, Name = "SuperAdmin" },
            new Role { Id = (int)UserRole.Admin, Name = "Admin" },
            new Role { Id = (int)UserRole.Student, Name = "Student" },
            new Role { Id = (int)UserRole.Lecturer, Name = "Lecturer" }
        );
        
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = new Guid("cc24a526-369e-46e0-a8d0-e179334ed456"), 
                Firstname = "Jan", 
                Lastname = "Kowalski", 
                Email = "root@euni.com", 
                RoleId = (int)UserRole.SuperAdmin,
                PasswordHash = "f7f376a1fcd0d0e11a10ed1b6577c99784d3a6bbe669b1d13fae43eb64634f6e", 
                Salt = new byte[2]
            },
            new User
            {
                Id = new Guid("cc24a526-369e-46e0-a8d0-e111111ed426"),
                Firstname = "Adam", 
                Lastname = "Nowak", 
                Email = "adam.nowak@pk.edu.pl", 
                RoleId = (int)UserRole.Lecturer,
                PasswordHash = "f7f386a1fcd0d0e11210ed1b6577c99784d3a6bce669b1d13fae43eb64634f6e", 
                Salt = new byte[2]
            }
        );
        
        modelBuilder.Entity<AcademicDepartment>().HasData(new AcademicDepartment
        {
            Id = 1,
            Abbr = "mech",
        });
         
        modelBuilder.Entity<EmploymentUnit>().HasData(new EmploymentUnit
        {
            Id = 1,
            Abbr = "M-04",
            AcademicDepartmentId = 1,
        });

        modelBuilder.Entity<Lecturer>().HasData(
            new Lecturer
            {
                Id = 1,
                UserId = new Guid("cc24a526-369e-46e0-a8d0-e111111ed426"),
                EmploymentUnitId = 1,
            }
        );
    }
}