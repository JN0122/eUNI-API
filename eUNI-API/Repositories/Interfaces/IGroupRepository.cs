using eUNI_API.Models.Dto.Group;
using eUNI_API.Models.Entities.Student;

namespace eUNI_API.Repositories.Interfaces;

public interface IGroupRepository
{
    public GroupDto GetGroupByClass(int classId);
    public Group GetGroupById(int groupId);
    public string GetGroupName(int classId);
}