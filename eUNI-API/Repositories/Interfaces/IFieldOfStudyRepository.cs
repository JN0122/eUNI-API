using eUNI_API.Models.Dto.FieldOfStudy;
using eUNI_API.Models.Entities.FieldOfStudy;
using eUNI_API.Models.Entities.OrganizationInfo;

namespace eUNI_API.Repositories.Interfaces;

public interface IFieldOfStudyRepository
{
    public Task<List<FieldOfStudy>> GetFieldsOfStudy(); 
    public FieldOfStudyLog GetFieldOfStudyLogById(int fieldOfStudyLogId);
    public FieldOfStudyInfoDto GetFieldOfStudyInfo(int fieldOfStudyLogId);
    public Task<List<FieldOfStudyInfoDto>?> GetFieldOfStudyLogs(int academicOrganizationId);
    public List<DayOff> GetDaysOff(int fieldOfStudyLogId);
    public IEnumerable<FieldOfStudyInfoDto> GetAllFieldOfStudyLogs();
}