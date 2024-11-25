using eUNI_API.Models.Dto.Group;

namespace eUNI_API.Repositories.Interfaces;

public interface IGroupRepository
{
    public GroupDto GetGroup(int classId);
}