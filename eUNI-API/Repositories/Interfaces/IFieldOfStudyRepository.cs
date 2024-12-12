using eUNI_API.Models.Dto.FieldOfStudy;
using eUNI_API.Models.Entities.FieldOfStudy;
using eUNI_API.Models.Entities.OrganizationInfo;

namespace eUNI_API.Repositories.Interfaces;

public interface IFieldOfStudyRepository
{
    public Task<List<FieldOfStudy>> GetFieldsOfStudy();
    public Task CreateFieldOfStudy(CreateFieldOfStudyRequest createFieldOfStudyRequest); 
    public Task UpdateFieldOfStudy(int id, CreateFieldOfStudyRequest createFieldOfStudyRequest); 
    public Task DeleteFieldOfStudy(int id); 
    public FieldOfStudyLog GetFieldOfStudyLogById(int fieldOfStudyLogId);
    public FieldOfStudyInfoDto GetFieldOfStudyInfo(int fieldOfStudyLogId);
    public Task<List<FieldOfStudyInfoDto>?> GetFieldOfStudyLogs(int academicOrganizationId);
    public List<DayOff> GetDaysOff(int fieldOfStudyLogId);
    public Task<List<FieldOfStudyLog>> GetFieldsOfStudyLogs();
    public FieldOfStudy GetFieldOfStudy(int id);
    public Task CreateFieldOfStudyLog(FieldOfStudy fieldOfStudy, OrganizationOfTheYear organization, byte semester);
    public Task DeleteFieldOfStudyLog(int fieldOfStudyLog);
}