using Domain.Entities;
using Domain.Models;

namespace Domain.Interfaces;

public interface IBiodataRepository
{
    Task<BiodataEntity?> FindByUserIdAsync(int userId);
    Task<BiodataEntity?> FindByIdAsync(int id);
    Task<BiodataEntity?> FindByNoKtpAsync(string noKtp);
    Task<BiodataEntity> CreateAsync(BiodataEntity biodata);
    Task<BiodataEntity> UpdateAsync(BiodataEntity biodata);
    Task DeleteAsync(int id);
    Task<PagedResult<BiodataEntity>> FindAllAsync(string? search, string? searchBy, int page, int limit);
}
