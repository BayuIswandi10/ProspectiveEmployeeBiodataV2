using Application.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/auth")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService) => _authService = authService;

    /// <summary>Registrasi akun baru</summary>
    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), 201)]
    [ProducesResponseType(typeof(ApiResponse<object>), 409)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var user = await _authService.RegisterAsync(request);
        return StatusCode(201, ApiResponse<UserDto>.Ok(user, "Registrasi berhasil"));
    }

    /// <summary>Login dan dapatkan JWT token</summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 401)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);
        return Ok(ApiResponse<AuthResponse>.Ok(result, "Login berhasil"));
    }

    /// <summary>Logout (JWT stateless — hapus token di client)</summary>
    [HttpPost("logout")]
    // [Authorize]
    public IActionResult Logout() =>
        Ok(ApiResponse<object>.Ok(null, "Logout berhasil"));
}
