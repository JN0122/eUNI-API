using eUNI_API.Models.Entities.User;

namespace eUNI_API.Services.Interfaces;

public interface IUsersService
{
    public Task<List<User>> GetUsers();
    public Task RemoveUser(Guid id);
}