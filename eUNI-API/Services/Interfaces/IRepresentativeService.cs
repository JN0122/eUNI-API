using eUNI_API.Models.Dto.Classes;
using eUNI_API.Models.Dto.FieldOfStudy;

namespace eUNI_API.Services.Interfaces;

public interface IRepresentativeService
{
    public Task<IEnumerable<FieldOfStudyInfoDto>?> FieldOfStudyLogsToEdit(Guid userId);
    public Task<IEnumerable<ClassDto>> GetClasses(int fieldOfStudyId);
    public Task CreateClass(CreateClassRequestDto classRequestDto);
    public Task UpdateClass(int id, CreateClassRequestDto classRequestDto);
    public Task DeleteClass(int id);
    public Task<IEnumerable<AssignmentDto>> GetAssignments(int fieldOfStudyLogId);
    public Task CreateAssignment(CreateAssignmentRequestDto assignmentRequestDto);
    public Task UpdateAssignment(int id, CreateAssignmentRequestDto assignmentRequestDto);
    public Task DeleteAssignment(int id);
}