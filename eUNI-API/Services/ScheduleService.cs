using eUNI_API.Data;
using eUNI_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eUNI_API.Services;

public class ScheduleService(AppDbContext context): IScheduleService
{
    private readonly AppDbContext _context = context;
    
    public async Task CalculateClassesDates(int classId)
    {
        Console.WriteLine(classId);
    }
}