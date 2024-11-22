using eUNI_API.Models.Entities.Auth;

namespace eUNI_API.Repositories.Interfaces;

public interface IUserRepository
{
    public Task<User> GetUserById(Guid id);
}