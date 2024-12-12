using eUNI_API.Models.Dto.Group;
using eUNI_API.Models.Dto.Student;
using eUNI_API.Models.Entities.Student;

namespace eUNI_API.Repositories.Interfaces;

public interface IStudentRepository
{
    public bool IsStudent(Guid userId);
    public Task<List<GroupDto>> GetGroups(int fieldOfStudyLogId, Guid userId);
    public IEnumerable<GroupDto> GetAllGroups(int fieldOfStudyLogId);
    public StudentFieldOfStudyDto? GetStudentCurrentFieldsOfStudy(Guid userId);
    public bool IsRepresentativeForFieldOfStudy(int fieldsOfStudyLogId, Guid userId);
    public bool IsRepresentative(Guid userId);
    public IEnumerable<StudentFieldOfStudyDto>? GetRepresentativeFieldsOfStudy(Guid userId);
    public StudentFieldsOfStudyLog GetStudentFieldOfStudyLog(int fieldOfStudyLogId, Guid userId);
    public StudentGroup? GetStudentGroup(int studentFieldOfStudyLogId, int groupType);
    public void JoinGroup(int studentFieldOfStudyLogId, int groupId);
    public void ChangeGroup(int studentGroupId, int groupId);
    public Task UpdateRepresentativeFields(Guid userId, List<int> representativeFieldsOfStudyLogIds);
    public Task SetCurrentFieldOfStudy(Guid userId, int fieldOfStudyId);
}