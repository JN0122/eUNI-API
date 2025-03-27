using eUNI_API.Models.Entities.Auth;

namespace eUNI_API.Repositories.Interfaces;

public interface IUserRepository
{
    public User? GetUserByEmail(string email);
    public void RemoveUserByEmail(string email);
    public Task<User> CreateUser(string firstName, string lastName, string email, string password, int roleId);
    public Task<User> GetUserById(Guid id);
    public void DeleteAllUsers();
}