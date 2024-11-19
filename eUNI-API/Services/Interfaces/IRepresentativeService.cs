using eUNI_API.Models.Dto.FieldOfStudy;

namespace eUNI_API.Services.Interfaces;

public interface IRepresentativeService
{
    public Task<List<FieldOfStudyInfoDto>?> GetFieldOfStudyLogToEdit(Guid userId);
    public Task<bool> IsRepresentative(Guid userId);
}