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
    public DbSet<RefreshToken> RefreshTokens { get; set; }
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
                Id = new Guid("c49c2319-2e87-45fe-be3b-1d9e724df781"), 
                FirstName = "Jan", 
                LastName = "Kowalski", 
                Email = "root@euni.com", 
                RoleId = (int)UserRole.SuperAdmin,
                PasswordHash =  "NTWxiNrLLLT2HkXuG9JiPYN0z5UN2eHW5gMsxbP4ATY=", 
                Salt = "mwmeU7TZlMdR/NMWAJMzrQ=="
            },
            new User
            {
                Id = new Guid("dd205297-4d6d-4ef3-a139-350c55518085"),
                FirstName = "Adam", 
                LastName = "Nowak", 
                Email = "adam.nowak@pk.edu.pl", 
                RoleId = (int)UserRole.Lecturer,
                PasswordHash = "4QQOqhGWUw4eHTZQdzStsPlGCtmiJ6Tz6bF9mwYXiVg=", 
                Salt = "f/5Yd/pRp65N6nbMHH8wlg=="
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
                UserId = new Guid("dd205297-4d6d-4ef3-a139-350c55518085"),
                EmploymentUnitId = 1,
            }
        );
    }
}