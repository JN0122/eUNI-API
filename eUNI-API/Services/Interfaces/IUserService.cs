using eUNI_API.Models.Dto;
using eUNI_API.Models.Entities.User;

namespace eUNI_API.Services.Interfaces;


public interface IUserService
{
    Task<User> CreateUser(CreateUser createUser);
    Task<User?> FindUserByClaimId(string claimId);
}