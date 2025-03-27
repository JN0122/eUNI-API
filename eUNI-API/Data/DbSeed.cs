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
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = (int)UserRole.Admin, Name = "Admin" },
            new Role { Id = (int)UserRole.Student, Name = "Student" }
        );

        var currentYear = DateTime.Now.Year;
        modelBuilder.Entity<Year>().HasData(
            new Year { Id = 1, Name=$"{currentYear}/{currentYear+1}" }
        );
        
        modelBuilder.Entity<OrganizationOfTheYear>().HasData(
            new OrganizationOfTheYear
            {
                Id = 1,
                FirstHalfOfYear = true,
                YearId = 1,
                StartDay = new DateOnly(currentYear, 10, 1),
                EndDay = new DateOnly(currentYear+1, 1, 26)
            }
        );
        
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
            new Group { Id = 19, Abbr = "3", Type = (int)GroupType.DeanGroup },
            new Group { Id = 20, Abbr = "4", Type = (int)GroupType.DeanGroup }
        );
        
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
            new Hour { Id = 15, StartHour = 19, StartMinute = 45, EndHour = 20, EndMinute = 30 },
            new Hour { Id = 16, StartHour = 20, StartMinute = 30, EndHour = 21, EndMinute = 15 }
        );
    }
}