using eUNI_API.Models.Dto.Group;
using eUNI_API.Models.Dto.Student;
using eUNI_API.Models.Entities.Student;

namespace eUNI_API.Repositories.Interfaces;

public interface IStudentRepository
{
    public Task<int?> GetStudentId(Guid userId);
    public Task<IEnumerable<GroupDto>?> GetGroups(int fieldOfStudyLogId, int studentId);
    public IEnumerable<GroupDto> GetAllGroups(int fieldOfStudyLogId);
    public Task<IEnumerable<StudentFieldOfStudyDto>?> GetStudentFieldsOfStudy(int studentId, int academicOrganizationId);
    public string? GetAlbumNumber(int studentId);
    public bool IsRepresentativeForFieldOfStudy(int fieldsOfStudyLogId, int studentId);
    public bool IsRepresentative(Guid userId, int academicOrganizationId);
    public IEnumerable<StudentFieldOfStudyDto>? GetRepresentativeFieldsOfStudy(int studentId, int academicOrganizationId);
    public StudentFieldsOfStudyLog GetStudentFieldOfStudyLog(int fieldOfStudyLogId, int studentId);
    public StudentGroup? GetStudentGroup(int studentFieldOfStudyLogId, int groupType);
    public void JoinGroup(int studentFieldOfStudyLogId, int groupId);
    public void ChangeGroup(int studentGroupId, int groupId);
}