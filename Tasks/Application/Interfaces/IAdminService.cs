using Domain.Models;

namespace Application.Interfaces;

public interface IAdminService
{
    Task<PagedResult<BiodataResponse>> GetAllCandidatesAsync(
        string? search, string? searchBy, int page, int limit);
    Task<BiodataResponse> GetCandidateDetailAsync(int id);
    Task DeleteCandidateAsync(int id);
}
