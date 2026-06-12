using Application.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/admin")]
[Produces("application/json")]
[Authorize(Roles = "admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;
    public AdminController(IAdminService adminService) => _adminService = adminService;

    /// <summary>Ambil semua kandidat dengan search dan pagination (Admin only)</summary>
    [HttpGet("candidates")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<BiodataResponse>>), 200)]
    public async Task<IActionResult> GetAllCandidates(
        [FromQuery] string? search,
        [FromQuery] string? searchBy,
        [FromQuery] int page = 1,
        [FromQuery] int limit = 10)
    {
        var result = await _adminService.GetAllCandidatesAsync(search, searchBy, page, limit);
        return Ok(ApiResponse<PagedResult<BiodataResponse>>.Ok(result, "Berhasil mengambil data kandidat"));
    }

    /// <summary>Detail kandidat berdasarkan ID (Admin only)</summary>
    [HttpGet("candidates/{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<BiodataResponse>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetCandidateDetail(int id)
    {
        var result = await _adminService.GetCandidateDetailAsync(id);
        return Ok(ApiResponse<BiodataResponse>.Ok(result, "Berhasil mengambil detail kandidat"));
    }

    /// <summary>Hapus kandidat berdasarkan ID (Admin only)</summary>
    [HttpDelete("candidates/{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<object>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteCandidate(int id)
    {
        await _adminService.DeleteCandidateAsync(id);
        return Ok(ApiResponse<object>.Ok(null, "Kandidat berhasil dihapus"));
    }
}
