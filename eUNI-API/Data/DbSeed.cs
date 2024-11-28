using eUNI_API.Enums;
using eUNI_API.Models.Entities.Auth;
using eUNI_API.Models.Entities.FieldOfStudy;
using eUNI_API.Models.Entities.OrganizationInfo;
using eUNI_API.Models.Entities.Student;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Data;

public static class DbSeed
{
    public static void seedDb(ModelBuilder modelBuilder)
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
        modelBuilder.Entity<Group>().HasKey(g => g.Id);
        modelBuilder.Entity<Group>().HasData(
            new Group { Id = 1, Abbr = "P01", Type = (int)GroupType.Project },
            new Group { Id = 2, Abbr = "P02", Type = (int)GroupType.Project },
            new Group { Id = 3, Abbr = "P03", Type = (int)GroupType.Project },
            new Group { Id = 4, Abbr = "P04", Type = (int)GroupType.Project },
            new Group { Id = 5, Abbr = "P05", Type = (int)GroupType.Project },
            new Group { Id = 6, Abbr = "W", Type = (int)GroupType.Lecture },
            new Group { Id = 7, Abbr = "K01", Type = (int)GroupType.Computer },
            new Group { Id = 8, Abbr = "K02", Type = (int)GroupType.Computer },
            new Group { Id = 9, Abbr = "K03", Type = (int)GroupType.Computer },
            new Group { Id = 10, Abbr = "K04", Type = (int)GroupType.Computer },
            new Group { Id = 11, Abbr = "K05", Type = (int)GroupType.Computer },
            new Group { Id = 12, Abbr = "L01", Type = (int)GroupType.Laboratory },
            new Group { Id = 13, Abbr = "L02", Type = (int)GroupType.Laboratory },
            new Group { Id = 14, Abbr = "L03", Type = (int)GroupType.Laboratory },
            new Group { Id = 15, Abbr = "L04", Type = (int)GroupType.Laboratory },
            new Group { Id = 16, Abbr = "L05", Type = (int)GroupType.Laboratory },
            new Group { Id = 17, Abbr = "1", Type = (int)GroupType.DeanGroup },
            new Group { Id = 18, Abbr = "2", Type = (int)GroupType.DeanGroup },
            new Group { Id = 19, Abbr = "3", Type = (int)GroupType.DeanGroup }
        );
        
        modelBuilder.Entity<Hour>()
            .Property(h => h.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<Hour>().HasData(
            new Hour { Id = 1, StartHour = 7, StartMinute = 30, EndHour = 8, EndMinute = 15 },
            new Hour { Id = 2, StartHour = 8, StartMinute = 15, EndHour = 9, EndMinute = 0 },
            new Hour { Id = 3, StartHour = 9, StartMinute = 15, EndHour = 10, EndMinute = 0 },
            new Hour { Id = 4, StartHour = 10, StartMinute = 0, EndHour = 10, EndMinute = 45 },
            new Hour { Id = 5, StartHour = 11, StartMinute = 0, EndHour = 11, EndMinute = 45 },
            new Hour { Id = 6, StartHour = 11, StartMinute = 45, EndHour = 12, EndMinute = 30 },
            new Hour { Id = 7, StartHour = 12, StartMinute = 45, EndHour = 13, EndMinute = 30 },
            new Hour { Id = 8, StartHour = 13, StartMinute = 30, EndHour = 14, EndMinute = 15 },
            new Hour { Id = 9, StartHour = 14, StartMinute = 30, EndHour = 15, EndMinute = 15 },
            new Hour { Id = 10, StartHour = 15, StartMinute = 15, EndHour = 16, EndMinute = 0 },
            new Hour { Id = 11, StartHour = 16, StartMinute = 15, EndHour = 17, EndMinute = 0 },
            new Hour { Id = 12, StartHour = 17, StartMinute = 0, EndHour = 17, EndMinute = 45 },
            new Hour { Id = 13, StartHour = 18, StartMinute = 0, EndHour = 18, EndMinute = 45 },
            new Hour { Id = 14, StartHour = 18, StartMinute = 45, EndHour = 19, EndMinute = 30 },
            new Hour { Id = 15, StartHour = 19, StartMinute = 45, EndHour = 20, EndMinute = 30 }
        );
        
        modelBuilder.Entity<Year>()
            .Property(y => y.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<Year>().HasData(
            new Year { Id = 1, Name="2024/2025" }
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
                SemesterCount = 7,
                IsFullTime = true
            },
            new FieldOfStudy
            {
                Id = 2,
                Abbr="S",
                Name="Pojazdy Samochodowe",
                StudiesCycle = 1,
                SemesterCount = 4,
                IsFullTime = false
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
            },
            new FieldOfStudyLog
            {
                Id = 2,
                FieldOfStudyId = 2,
                OrganizationsOfTheYearId = 1,
                Semester = 5
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
                AlbumNumber = "D/123231/"
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