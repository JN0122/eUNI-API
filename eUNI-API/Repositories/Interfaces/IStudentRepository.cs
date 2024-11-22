using eUNI_API.Models.Dto.FieldOfStudy;

namespace eUNI_API.Repositories.Interfaces;

public interface IStudentRepository
{
    public Task<int> GetStudentId(Guid userId);
    public Task<List<int>?> GetStudentGroupIds(int fieldOfStudyId, Guid userId);
    public Task<List<FieldOfStudyInfoDto>?> GetStudentFieldsOfStudy(Guid userId);
    public string? GetAlbumNumber(int studentId);
}