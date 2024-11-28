using eUNI_API.Models.Dto.Classes;
using eUNI_API.Models.Entities.FieldOfStudy;

namespace eUNI_API.Repositories.Interfaces;

public interface IClassesRepository
{
    public Class GetClassById(int classId);
    public IEnumerable<HourDto> GetHour();
    public IEnumerable<Class> GetGroupsClasses(int fieldOfStudyLogId, IEnumerable<int> groupIds);
    public IEnumerable<ClassDate> GetClassDates(int classId);
}