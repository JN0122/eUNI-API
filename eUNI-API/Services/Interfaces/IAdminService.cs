using eUNI_API.Models.Dto;
using eUNI_API.Models.Entities.Auth;

namespace eUNI_API.Services.Interfaces;

public interface IAdminService
{
    public Task<List<User>> GetUsers();
    public Task RemoveUser(Guid id);
    public Task CreateUser(CreateUserRequestDto createUserRequestDto);
    public Task UpdateUser(Guid userId, UpdateUserRequestDto updateUserRequestDto);
}