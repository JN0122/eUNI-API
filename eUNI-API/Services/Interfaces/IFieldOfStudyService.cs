using eUNI_API.Models.Dto.FieldOfStudy;
using eUNI_API.Models.Dto.Group;

namespace eUNI_API.Services.Interfaces;

public interface IFieldOfStudyService
{
    public Task<IEnumerable<FieldOfStudyDto>> GetFieldsOfStudy();
    public Task CreateFieldOfStudy(CreateFieldOfStudyRequest createFieldOfStudyRequest);
    public Task UpdateFieldOfStudy(int id, CreateFieldOfStudyRequest createFieldOfStudyRequest);
    public Task DeleteFieldOfStudy(int id);
    public Task<IEnumerable<GroupDto>> GetGroups(int fieldOfStudyLogId);
    public Task<List<FieldOfStudyInfoDto>> GetFieldsOfStudyLogsInfoDtos();
    public Task CreateFieldOfStudyLog(CreateFieldOfStudyLogRequest createFieldOfStudyLogRequest);
    public Task DeleteFieldOfStudyLog(int id);
}