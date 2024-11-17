using eUNI_API.Models.Dto;
using eUNI_API.Models.Entities.Auth;

namespace eUNI_API.Services.Interfaces;

public interface IUsersService
{
    public Task<List<User>> GetUsers();
    public Task RemoveUser(Guid id);
    public Task CreateUser(CreateUserRequestDto createUserRequestDto);
    public Task<User> GetUserById(Guid id);
    public Task UpdateUser(User user, UpdateUserRequestDto updateUserRequestDto);
}