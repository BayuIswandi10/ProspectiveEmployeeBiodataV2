using Domain.Models;

namespace Application.Interfaces;

public interface IBiodataService
{
    Task<BiodataResponse> GetMyBiodataAsync(int userId);
    Task<BiodataResponse> CreateBiodataAsync(int userId, BiodataRequest request);
    Task<BiodataResponse> UpdateBiodataAsync(int id, int userId, string userRole, BiodataRequest request);
    Task DeleteBiodataAsync(int id, int userId, string userRole);
}
