using eUNI_API.Models.Dto.Classes;
using eUNI_API.Models.Entities.FieldOfStudy;

namespace eUNI_API.Repositories.Interfaces;

public interface IClassesRepository
{
    public Class GetClassById(int classId);
    public IEnumerable<HourDto> GetHour();
    public IQueryable<Class> GetGroupsClasses(int fieldOfStudyLogId, IEnumerable<int> groupIds);
    public IQueryable<Class> GetClasses(int fieldOfStudyLogId);
    public IQueryable<ClassDate> GetClassDates(int classId);
    public List<ClassDto> GetClassesDto(IQueryable<Class> classEntity);
    public Task DeleteAllClasses();
}