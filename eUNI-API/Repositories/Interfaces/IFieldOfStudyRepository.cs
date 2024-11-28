using eUNI_API.Models.Dto.FieldOfStudy;
using eUNI_API.Models.Entities.FieldOfStudy;

namespace eUNI_API.Repositories.Interfaces;

public interface IFieldOfStudyRepository
{
    public FieldOfStudyLog GetFieldOfStudyLogById(int fieldOfStudyLogId);
    public Task<List<FieldOfStudyInfoDto>?> GetFieldOfStudyLogs(int academicOrganizationId);
}