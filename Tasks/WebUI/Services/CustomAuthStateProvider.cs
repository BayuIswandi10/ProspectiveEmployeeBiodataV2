using Blazored.LocalStorage;
using Domain.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace WebUI.Services;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly HttpClient _http;

    private AuthenticationState _cachedState = new(new ClaimsPrincipal(new ClaimsIdentity()));
    public bool IsInitializing { get; private set; } = true;

    public CustomAuthStateProvider(ILocalStorageService localStorage, HttpClient http)
    {
        _localStorage = localStorage;
        _http = http;
    }

    private void SetHeaders(string token)
    {
        _http.DefaultRequestHeaders.Remove("X-User-Id");
        _http.DefaultRequestHeaders.Remove("X-User-Role");
        _http.DefaultRequestHeaders.Authorization = null;

        if (!string.IsNullOrWhiteSpace(token))
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return Task.FromResult(_cachedState);
    }

    public async Task LoginAsync(string token)
    {
        await _localStorage.SetItemAsStringAsync("jwt_token", token);
        UpdateState(token);
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync("jwt_token");
        _cachedState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        SetHeaders(null!);
        NotifyAuthenticationStateChanged(Task.FromResult(_cachedState));
    }

    public async Task TryInitializeAsync()
    {
        if (!IsInitializing) return;
        try
        {
            var token = await _localStorage.GetItemAsStringAsync("jwt_token");
            if (!string.IsNullOrWhiteSpace(token))
            {
                UpdateState(token);
            }
        }
        catch { }
        finally
        {
            IsInitializing = false;
            NotifyAuthenticationStateChanged(Task.FromResult(_cachedState)); // Force re-render of components relying on AuthState
        }
    }

    private void UpdateState(string token)
    {
        try
        {
            var claims = ParseClaimsFromJwt(token);
            if (!claims.Any()) throw new Exception("Invalid session");

            SetHeaders(token);
            var identity = new ClaimsIdentity(claims, "jwt");
            _cachedState = new AuthenticationState(new ClaimsPrincipal(identity));
        }
        catch
        {
            _cachedState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            SetHeaders(null!);
        }
        NotifyAuthenticationStateChanged(Task.FromResult(_cachedState));
    }

    private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var claims = new List<Claim>();
        var parts = jwt.Split('.');
        if (parts.Length != 3) return claims; // Not a JWT, maybe an old session? Let it fail

        var payload = parts[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        if (keyValuePairs != null)
        {
            foreach (var kvp in keyValuePairs)
            {
                if (kvp.Value is JsonElement element && element.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in element.EnumerateArray())
                    {
                        claims.Add(new Claim(kvp.Key, item.ToString()));
                    }
                }
                else
                {
                    claims.Add(new Claim(kvp.Key, kvp.Value?.ToString() ?? ""));
                }
            }
        }
        return claims;
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }
}
