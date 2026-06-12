using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;
    public UserRepository(AppDbContext db) => _db = db;

    public async Task<UserEntity?> FindByEmailAsync(string email) =>
        await _db.Users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<UserEntity?> FindByIdAsync(int id) =>
        await _db.Users.FindAsync(id);

    public async Task<UserEntity> CreateAsync(UserEntity user)
    {
        user.CreatedAt = DateTime.Now;
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }
}
