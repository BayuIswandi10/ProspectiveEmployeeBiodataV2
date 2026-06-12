using Domain.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace WebUI.Services;

public class AuthApiService
{
    private readonly HttpClient _http;
    private static readonly JsonSerializerOptions _opts =
        new() { PropertyNameCaseInsensitive = true };

    public AuthApiService(HttpClient http) => _http = http;

    public async Task<ApiResponse<AuthResponse>?> LoginAsync(LoginRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/auth/login", request);
        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode && string.IsNullOrWhiteSpace(content))
            throw new Exception($"Server returned {response.StatusCode} without content.");
            
        try {
            return JsonSerializer.Deserialize<ApiResponse<AuthResponse>>(content, _opts);
        } catch {
            throw new Exception($"Gagal memproses respons. Status: {response.StatusCode}, Content: {content}");
        }
    }

    public async Task<ApiResponse<UserDto>?> RegisterAsync(RegisterRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/auth/register", request);
        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode && string.IsNullOrWhiteSpace(content))
            throw new Exception($"Server returned {response.StatusCode} without content.");

        try {
            return JsonSerializer.Deserialize<ApiResponse<UserDto>>(content, _opts);
        } catch {
            throw new Exception($"Gagal memproses respons. Status: {response.StatusCode}, Content: {content}");
        }
    }
}
