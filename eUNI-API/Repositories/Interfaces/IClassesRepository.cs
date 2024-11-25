using eUNI_API.Models.Dto.Classes;

namespace eUNI_API.Repositories.Interfaces;

public interface IClassesRepository
{
    public IEnumerable<HourDto> GetHour();
}