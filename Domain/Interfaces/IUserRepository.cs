using Domain.Entities;

namespace Domain.Interfaces;

public interface IUserRepository
{
    Task<UserEntity?> FindByEmailAsync(string email);
    Task<UserEntity?> FindByIdAsync(int id);
    Task<UserEntity> CreateAsync(UserEntity user);
}
