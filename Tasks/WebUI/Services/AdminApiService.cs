using Domain.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace WebUI.Services;

public class AdminApiService
{
    private readonly HttpClient _http;
    private static readonly JsonSerializerOptions _opts =
        new() { PropertyNameCaseInsensitive = true };

    public AdminApiService(HttpClient http) => _http = http;

    public async Task<ApiResponse<PagedResult<BiodataResponse>>?> GetAllCandidatesAsync(
        string? search, string? searchBy, int page = 1, int limit = 10)
    {
        var query = $"api/admin/candidates?page={page}&limit={limit}";
        if (!string.IsNullOrWhiteSpace(search)) query += $"&search={Uri.EscapeDataString(search)}";
        if (!string.IsNullOrWhiteSpace(searchBy)) query += $"&searchBy={searchBy}";

        var response = await _http.GetAsync(query);
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ApiResponse<PagedResult<BiodataResponse>>>(content, _opts);
    }

    public async Task<ApiResponse<BiodataResponse>?> GetCandidateDetailAsync(int id)
    {
        var response = await _http.GetAsync($"api/admin/candidates/{id}");
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ApiResponse<BiodataResponse>>(content, _opts);
    }

    public async Task<bool> DeleteCandidateAsync(int id)
    {
        var response = await _http.DeleteAsync($"api/admin/candidates/{id}");
        return response.IsSuccessStatusCode;
    }
}
