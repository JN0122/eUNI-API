using eUNI_API.Enums;
using eUNI_API.Models.Entities.Auth;
using eUNI_API.Models.Entities.FieldOfStudy;
using eUNI_API.Models.Entities.OrganizationInfo;
using eUNI_API.Models.Entities.Student;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { } 
    
    // Auth
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<PasswordResetLog> PasswordResetLogs { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    
    //Student
    public DbSet<Group> Groups { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<StudentFieldsOfStudyLog> StudentFieldsOfStudyLogs { get; set; }
    public DbSet<StudentGroup> StudentGroups { get; set; }
    
    //OrganizationInfo
    public DbSet<DayOff> DaysOff { get; set; }
    public DbSet<Hour> Hours { get; set; }
    public DbSet<OrganizationOfTheYear> OrganizationsOfTheYear { get; set; }
    public DbSet<Year> Years { get; set; }
    
    //FieldOfStudy
    public DbSet<Class> Classes { get; set; }
    public DbSet<FieldOfStudy> FieldOfStudies { get; set; }
    public DbSet<FieldOfStudyLog> FieldOfStudyLogs { get; set; }
    public DbSet<Assignment> Assignments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        DbSeed.seedDb(modelBuilder);
    }
}