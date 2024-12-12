using eUNI_API.Enums;
using eUNI_API.Models.Entities.Auth;
using eUNI_API.Models.Entities.FieldOfStudy;
using eUNI_API.Models.Entities.OrganizationInfo;
using eUNI_API.Models.Entities.Student;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    // Auth
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<PasswordResetLog> PasswordResetLogs { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    
    //Student
    public DbSet<Group> Groups { get; set; }
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
    public DbSet<ClassDate> ClassDates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>()
            .Property(r => r.Id)
            .ValueGeneratedOnAdd();
        
        modelBuilder.Entity<Group>()
            .Property(g => g.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<Group>()
            .HasKey(g => g.Id);
        
        modelBuilder.Entity<Hour>()
            .Property(h => h.Id)
            .ValueGeneratedOnAdd();
        
        modelBuilder.Entity<Year>()
            .Property(y => y.Id)
            .ValueGeneratedOnAdd();
        
        modelBuilder.Entity<OrganizationOfTheYear>()
            .Property(o => o.Id)
            .ValueGeneratedOnAdd();
        
        modelBuilder.Entity<FieldOfStudy>()
            .Property(f => f.Id)
            .ValueGeneratedOnAdd();
        
        modelBuilder.Entity<FieldOfStudyLog>()
            .Property(f => f.Id)
            .ValueGeneratedOnAdd();
        
        modelBuilder.Entity<FieldOfStudyLog>()
            .HasOne(fl => fl.FieldOfStudy)
            .WithMany(f => f.FieldOfStudyLogs)
            .HasForeignKey(fl => fl.FieldOfStudyId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<FieldOfStudyLog>()
            .HasOne(fl => fl.OrganizationsOfTheYear)
            .WithMany(o => o.FieldOfStudyLogs)
            .HasForeignKey(fl => fl.OrganizationsOfTheYearId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Class>()
            .Property(c => c.Id)
            .ValueGeneratedOnAdd();
        
        modelBuilder.Entity<StudentFieldsOfStudyLog>()
            .Property(s => s.Id)
            .ValueGeneratedOnAdd();
        
        modelBuilder.Entity<StudentGroup>()
            .Property(s => s.Id)
            .ValueGeneratedOnAdd();
        
        DbSeed.seedDb(modelBuilder);
    }
}