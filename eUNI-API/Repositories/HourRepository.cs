using eUNI_API.Data;
using eUNI_API.Models.Entities.OrganizationInfo;
using eUNI_API.Repositories.Interfaces;

namespace eUNI_API.Repositories;

public class HourRepository(AppDbContext context): IHourRepository
{
    private readonly AppDbContext _context = context;
    
    public Hour GetHourById(int hourId)
    {
        var hour = _context.Hours.FirstOrDefault(h => h.Id == hourId);
        if(hour == null) throw new ArgumentException($"Hour with id {hourId} not found");
        return hour;
    }

    public IEnumerable<Hour> GetHoursRange(int startId, int endId)
    {
        if(startId > endId) throw new Exception("StartHourId cannot be greater than EndHourId");
        return _context.Hours.Where(h=>h.Id >= startId && h.Id <= endId);
    }
}