using eUNI_API.Models.Dto.FieldOfStudy;

namespace eUNI_API.Repositories.Interfaces;

public interface IStudentRepository
{
    public Task<int> GetStudentId(Guid userId);
    public Task<IEnumerable<int>?> GetStudentGroupIds(int fieldOfStudyLogId, int studentId);
    public Task<IEnumerable<FieldOfStudyInfoDto>?> GetStudentFieldsOfStudy(int studentId);
    public string? GetAlbumNumber(int studentId);
}