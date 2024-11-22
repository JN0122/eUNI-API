using eUNI_API.Models.Dto.Student;

namespace eUNI_API.Repositories.Interfaces;

public interface IStudentRepository
{
    public Task<int?> GetStudentId(Guid userId);
    public Task<IEnumerable<int>?> GetStudentGroupIds(int fieldOfStudyLogId, int studentId);
    public Task<IEnumerable<StudentFieldOfStudyDto>?> GetStudentFieldsOfStudy(int studentId, int academicOrganizationId);
    public string? GetAlbumNumber(int studentId);
    public bool IsRepresentativeForFieldOfStudy(int fieldsOfStudyLogId, int studentId);
    public bool IsRepresentative(Guid userId, int academicOrganizationId);
    public IEnumerable<StudentFieldOfStudyDto>? GetRepresentativeFieldsOfStudy(int studentId, int academicOrganizationId);
}