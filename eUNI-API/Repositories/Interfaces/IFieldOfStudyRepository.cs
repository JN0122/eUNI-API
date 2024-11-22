using eUNI_API.Models.Dto.FieldOfStudy;

namespace eUNI_API.Repositories.Interfaces;

public interface IFieldOfStudyRepository
{
    public Task<List<FieldOfStudyInfoDto>?> GetFieldOfStudyLogs(int academicOrganizationId);
}