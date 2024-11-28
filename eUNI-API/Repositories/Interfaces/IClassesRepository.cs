using eUNI_API.Models.Dto.Classes;
using eUNI_API.Models.Entities.FieldOfStudy;

namespace eUNI_API.Repositories.Interfaces;

public interface IClassesRepository
{
    public Class GetClassById(int classId);
    public IEnumerable<HourDto> GetHour();
}