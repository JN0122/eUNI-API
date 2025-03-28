using eUNI_API.Models.Dto;
using eUNI_API.Models.Dto.User;
using eUNI_API.Models.Entities.Auth;

namespace eUNI_API.Services.Interfaces;

public interface IAdminService
{
    public Task<List<User>> GetUsers();
    public Task RemoveUser(Guid id);
    public Task<User> CreateUser(CreateUserRequestDto createUserRequestDto);
    public Task UpdateUser(Guid userId, UpdateUserRequestDto updateUserRequestDto);
}