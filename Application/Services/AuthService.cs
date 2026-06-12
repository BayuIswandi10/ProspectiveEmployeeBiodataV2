using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepo;
    private readonly IConfiguration _config;

    public AuthService(IUserRepository userRepo, IConfiguration config)
    {
        _userRepo = userRepo;
        _config = config;
    }

    public async Task<UserDto> RegisterAsync(RegisterRequest request)
    {
        var existing = await _userRepo.FindByEmailAsync(request.Email);
        if (existing is not null)
            throw new InvalidOperationException("Email sudah terdaftar");

        var hashed = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var user = await _userRepo.CreateAsync(new UserEntity
        {
            Email = request.Email,
            Password = hashed,
            Role = request.Role ?? "user"
        });

        return new UserDto { Id = user.Id, Email = user.Email, Role = user.Role };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userRepo.FindByEmailAsync(request.Email)
            ?? throw new UnauthorizedAccessException("Email atau password salah");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            throw new UnauthorizedAccessException("Email atau password salah");

        var userDto = new UserDto { Id = user.Id, Email = user.Email, Role = user.Role };
        return new AuthResponse
        {
            Token = GenerateJwtToken(userDto),
            User = userDto
        };
    }

    private string GenerateJwtToken(UserDto user)
    {
        var jwtKey = _config["Jwt:Key"] ?? "IniAdalahKunciRahasiaYangSangatPanjangUntukJWT123!";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
