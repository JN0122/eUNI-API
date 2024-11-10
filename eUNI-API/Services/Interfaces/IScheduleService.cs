namespace eUNI_API.Services.Interfaces;

public interface IScheduleService
{
    public Task CalculateClassesDates(int classId);
}