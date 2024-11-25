using eUNI_API.Data;
using eUNI_API.Models.Dto.Classes;
using eUNI_API.Repositories.Interfaces;

namespace eUNI_API.Repositories;

public class ClassesRepository(AppDbContext context): IClassesRepository
{
    private readonly AppDbContext _context = context;
    
    public IEnumerable<HourDto> GetHour()
    {
        return _context.Hours.ToList().Select(h => new HourDto
        {
            HourId = h.Id,
            HourName = h.HourInterval
        });
    }
}