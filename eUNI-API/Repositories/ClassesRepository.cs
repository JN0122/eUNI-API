using eUNI_API.Data;
using eUNI_API.Enums;
using eUNI_API.Exception;
using eUNI_API.Helpers;
using eUNI_API.Models.Dto.Classes;
using eUNI_API.Models.Entities.FieldOfStudy;
using eUNI_API.Models.Entities.Student;
using eUNI_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Repositories;

public class ClassesRepository(AppDbContext context): IClassesRepository
{
    private readonly AppDbContext _context = context;

    public Class GetClassById(int classId)
    {
        var classEntity = _context.Classes.FirstOrDefault(c => c.Id == classId);
        if(classEntity == null)
            throw new HttpNotFoundException($"Class not found: {classId}");
        return classEntity;
    }

    public IEnumerable<HourDto> GetHour()
    {
        return _context.Hours.Select(ConvertDtos.ToHourDto);
    }

    public IQueryable<Class> GetGroupsClasses(int fieldOfStudyLogId, IEnumerable<int> groupIds)
    {
        return _context.Classes.Where(c => c.FieldOfStudyLogId == fieldOfStudyLogId && groupIds.Any(g => g == c.GroupId));
    }

    public IQueryable<Class> GetClasses(int fieldOfStudyLogId)
    {
        return _context.Classes.Where(c => c.FieldOfStudyLogId == fieldOfStudyLogId);
    }

    public IQueryable<ClassDate> GetClassDates(int classId)
    {
        return _context.ClassDates.Where(c =>c.ClassId == classId);
    }

    private string GetGroupName(Group group, int classId)
    {
        if (group.Type != (int)GroupType.DeanGroup)
            return group.Abbr;
        var classEntity = _context.Classes
            .AsNoTracking()
            .Include(c => c.FieldOfStudyLog)
            .ThenInclude(f => f.FieldOfStudy)
            .FirstOrDefault(c => c.Id == classId);
        if(classEntity == null) throw new HttpNotFoundException($"Class not found: {classId}");
        return classEntity.FieldOfStudyLog.FieldOfStudy.Abbr + group.Abbr;
    }

    public List<ClassDto> GetClassesDto(IQueryable<Class> classEntities)
    {
        var classes = classEntities
            .Include(c=>c.EndHour)
            .Include(c=>c.StartHour)
            .Include(c=>c.ClassDates)
            .Include(c => c.Group)
            .ToList();

        return classes.Select(classEntity => new ClassDto
            {
                Id = classEntity.Id,
                Name = classEntity.Name,
                GroupId = classEntity.GroupId,
                GroupName = GetGroupName(classEntity.Group, classEntity.Id),
                EndHour = ConvertDtos.ToHourDto(classEntity.EndHour),
                StartHour = ConvertDtos.ToHourDto(classEntity.StartHour),
                FieldOfStudyLogId = classEntity.FieldOfStudyLogId,
                ClassRoom = classEntity.Room,
                Dates = classEntity.ClassDates.Select(cd => cd.Date)
            })
            .ToList();
    }

    public void DeleteAllClasses()
    {
        _context.ClassDates.RemoveRange(_context.ClassDates);
        _context.Classes.RemoveRange(_context.Classes);
        _context.SaveChanges();
    }
}