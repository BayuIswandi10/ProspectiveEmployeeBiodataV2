using Application.Interfaces;
using Domain.Interfaces;
using Domain.Models;

namespace Application.Services;

public class AdminService : IAdminService
{
    private readonly IBiodataRepository _biodataRepo;

    public AdminService(IBiodataRepository biodataRepo)
    {
        _biodataRepo = biodataRepo;
    }

    public async Task<PagedResult<BiodataResponse>> GetAllCandidatesAsync(
        string? search, string? searchBy, int page, int limit)
    {
        var result = await _biodataRepo.FindAllAsync(search, searchBy, page, limit);
        return new PagedResult<BiodataResponse>
        {
            Data = result.Data.Select(BiodataService.MapToResponse).ToList(),
            Total = result.Total,
            Page = result.Page,
            Limit = result.Limit
        };
    }

    public async Task<BiodataResponse> GetCandidateDetailAsync(int id)
    {
        var biodata = await _biodataRepo.FindByIdAsync(id)
            ?? throw new KeyNotFoundException("Kandidat tidak ditemukan");
        return BiodataService.MapToResponse(biodata);
    }

    public async Task DeleteCandidateAsync(int id)
    {
        var biodata = await _biodataRepo.FindByIdAsync(id)
            ?? throw new KeyNotFoundException("Kandidat tidak ditemukan");
        await _biodataRepo.DeleteAsync(biodata.Id);
    }
}
