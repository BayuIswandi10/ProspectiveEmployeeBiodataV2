using Application.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers;

[ApiController]
[Route("api/biodata")]
[Produces("application/json")]
[Authorize]
public class BiodataController : ControllerBase
{
    private readonly IBiodataService _biodataService;
    public BiodataController(IBiodataService biodataService) => _biodataService = biodataService;

    private int UserId => int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : 0;
    private string UserRole => User.FindFirstValue(ClaimTypes.Role) ?? "user";

    /// <summary>Ambil biodata milik user yang sedang login</summary>
    [HttpGet("my")]
    [ProducesResponseType(typeof(ApiResponse<BiodataResponse>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetMyBiodata()
    {
        var result = await _biodataService.GetMyBiodataAsync(UserId);
        return Ok(ApiResponse<BiodataResponse>.Ok(result, "Berhasil mengambil biodata"));
    }

    /// <summary>Buat biodata baru</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<BiodataResponse>), 201)]
    [ProducesResponseType(typeof(ApiResponse<object>), 409)]
    public async Task<IActionResult> CreateBiodata([FromBody] BiodataRequest request)
    {
        var result = await _biodataService.CreateBiodataAsync(UserId, request);
        return StatusCode(201, ApiResponse<BiodataResponse>.Ok(result, "Biodata berhasil disimpan"));
    }

    /// <summary>Update biodata berdasarkan ID</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<BiodataResponse>), 200)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateBiodata(int id, [FromBody] BiodataRequest request)
    {
        var result = await _biodataService.UpdateBiodataAsync(id, UserId, UserRole, request);
        return Ok(ApiResponse<BiodataResponse>.Ok(result, "Biodata berhasil diupdate"));
    }

    /// <summary>Hapus biodata berdasarkan ID</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<object>), 200)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteBiodata(int id)
    {
        await _biodataService.DeleteBiodataAsync(id, UserId, UserRole);
        return Ok(ApiResponse<object>.Ok(null, "Biodata berhasil dihapus"));
    }
}
