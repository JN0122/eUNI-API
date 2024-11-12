using eUNI_API.Models.Dto;

namespace eUNI_API.Services.Interfaces;

public interface IScheduleService
{
    public Task<List<DateOnly>> CalculateClassesDates(ClassesToCalculateDto classesToCalculateDto);
}