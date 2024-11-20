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
        modelBuilder.Entity<Role>()
            .Property(r => r.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = (int)UserRole.Admin, Name = "Admin" },
            new Role { Id = (int)UserRole.Student, Name = "Student" }
        );
        
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = new Guid("c49c2319-2e87-45fe-be3b-1d9e724df781"), 
                FirstName = "Jan", 
                LastName = "Kowalski", 
                Email = "root@euni.com", 
                RoleId = (int)UserRole.Admin,
                PasswordHash =  "NTWxiNrLLLT2HkXuG9JiPYN0z5UN2eHW5gMsxbP4ATY=", 
                Salt = "mwmeU7TZlMdR/NMWAJMzrQ=="
            },
            new User
            {
                Id = new Guid("dd205297-4d6d-4ef3-a139-350c55518085"),
                FirstName = "Adam", 
                LastName = "Nowak", 
                Email = "adam.nowak@pk.edu.pl", 
                RoleId = (int)UserRole.Student,
                PasswordHash = "4QQOqhGWUw4eHTZQdzStsPlGCtmiJ6Tz6bF9mwYXiVg=", 
                Salt = "f/5Yd/pRp65N6nbMHH8wlg=="
            }
        );

        modelBuilder.Entity<Group>()
            .Property(g => g.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<Group>().HasData(
            new Group
            {
                Id = 1,
                Abbr = "P01",
                Type = (int)GroupType.Project
            },
            new Group
            {
                Id = 2,
                Abbr = "W",
                Type = (int)GroupType.Lecture
            },
            new Group
            {
                Id = 3,
                Abbr = "K02",
                Type = (int)GroupType.Computer
            },
            new Group
            {
                Id = 4,
                Abbr = "K01",
                Type = (int)GroupType.Computer
            },
            new Group
            {
                Id = 5,
                Abbr = "L01",
                Type = (int)GroupType.Laboratory
            }
        );

        modelBuilder.Entity<Hour>()
            .Property(h => h.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<Hour>().HasData(
            new Hour
            {
                Id = 1,
                HourInterval="7:30 - 8:15"
            },
            new Hour
            {
                Id = 2,
                HourInterval="8:15 - 9:00"
            },
            new Hour
            {
                Id = 3,
                HourInterval="9:15 - 10:00"
            },
            new Hour
            {
                Id = 4,
                HourInterval="10:00 - 10:45"
            }
        );
        
        modelBuilder.Entity<Year>()
            .Property(y => y.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<Year>().HasData(
            new Year
            {
                Id = 1,
                Name="2024/2025"
            }
        );
        
        modelBuilder.Entity<OrganizationOfTheYear>()
            .Property(o => o.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<OrganizationOfTheYear>().HasData(
            new OrganizationOfTheYear
            {
                Id = 1,
                YearId = 1,
                FirstHalfOfYear = true,
                StartDay = new DateOnly(2024, 10, 1),
                EndDay = new DateOnly(2025, 1, 26),
            }    
        );

        modelBuilder.Entity<FieldOfStudy>()
            .Property(f => f.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<FieldOfStudy>().HasData(
            new FieldOfStudy
            {
                Id = 1,
                Abbr="K",
                Name="Informatyka Stosowana",
                StudiesCycle = 1,
                SemesterCount = 7
            }
        );

        modelBuilder.Entity<FieldOfStudyLog>()
            .Property(f => f.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<FieldOfStudyLog>().HasData(
            new FieldOfStudyLog
            {
                Id = 1,
                FieldOfStudyId = 1,
                OrganizationsOfTheYearId = 1,
                Semester = 7
            }    
        );
        
        modelBuilder.Entity<Class>()
            .Property(c => c.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<Class>().HasData(
            new Class
            {
                Id = 1,
                FieldOfStudyLogId = 1,
                Name = "Wytrzymałość materiałów",
                Room="A103",
                IsOddWeek = true,
                WeekDay = WeekDay.Monday,
                GroupId = 2,
                StartHourId = 1,
                EndHourId = 3,
            }    
        );

        modelBuilder.Entity<Student>()
            .Property(s => s.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<Student>().HasData(
            new Student
            {
                Id = 1,
                UserId = new Guid("dd205297-4d6d-4ef3-a139-350c55518085"),
                AlbumNumber = "144863"
            }
        );
        
        modelBuilder.Entity<StudentFieldsOfStudyLog>()
            .Property(s => s.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<StudentFieldsOfStudyLog>().HasData(
            new StudentFieldsOfStudyLog
            {
                Id = 1,
                FieldsOfStudyLogId = 1,
                StudentId = 1,
                IsRepresentative  = true
            }    
        );
        
        modelBuilder.Entity<StudentGroup>()
            .Property(s => s.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<StudentGroup>().HasData(
            new StudentGroup{
                Id = 1,
                GroupId = 1,
                StudentsFieldsOfStudyLogId = 1,
            }
        );
        
        modelBuilder.Entity<Assignment>()
            .Property(a => a.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<Assignment>().HasData(
           new Assignment
           {
               Id = 1,
               ClassId = 1,
               Name = "Sprawozdanie 1",
               DeadlineDate = new DateOnly(2024, 11, 15)
           }
        );
    }
}