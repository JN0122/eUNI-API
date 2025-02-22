using System.Collections;
using eUNI_API.Models.Entities.OrganizationInfo;

namespace eUNI_API.Repositories.Interfaces;

public interface IHourRepository
{
    public Hour GetHourById(int hourId);
    public IEnumerable<Hour> GetHoursRange(int startId, int endId);
}