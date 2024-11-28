using eUNI_API.Data;
using eUNI_API.Helpers;
using eUNI_API.Models.Dto.Classes;
using eUNI_API.Models.Entities.FieldOfStudy;
using eUNI_API.Repositories.Interfaces;

namespace eUNI_API.Repositories;

public class ClassesRepository(AppDbContext context): IClassesRepository
{
    private readonly AppDbContext _context = context;

    public Class GetClassById(int classId)
    {
        var classEntity = _context.Classes.FirstOrDefault(c => c.Id == classId);
        if(classEntity == null)
            throw new ArgumentException($"Class not found: {classId}");
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
}