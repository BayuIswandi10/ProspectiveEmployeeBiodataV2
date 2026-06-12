using Domain.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace WebUI.Services;

public class BiodataApiService
{
    private readonly HttpClient _http;
    private static readonly JsonSerializerOptions _opts =
        new() { PropertyNameCaseInsensitive = true };

    public BiodataApiService(HttpClient http) => _http = http;

    public async Task<ApiResponse<BiodataResponse>?> GetMyBiodataAsync()
    {
        var response = await _http.GetAsync("api/biodata/my");
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ApiResponse<BiodataResponse>>(content, _opts);
    }

    public async Task<ApiResponse<BiodataResponse>?> CreateBiodataAsync(BiodataRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/biodata", request);
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ApiResponse<BiodataResponse>>(content, _opts);
    }

    public async Task<ApiResponse<BiodataResponse>?> UpdateBiodataAsync(int id, BiodataRequest request)
    {
        var response = await _http.PutAsJsonAsync($"api/biodata/{id}", request);
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ApiResponse<BiodataResponse>>(content, _opts);
    }

    public async Task<ApiResponse<object>?> DeleteBiodataAsync(int id)
    {
        var response = await _http.DeleteAsync($"api/biodata/{id}");
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ApiResponse<object>>(content, _opts);
    }
}
