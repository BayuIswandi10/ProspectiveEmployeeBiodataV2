using Domain.Entities;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class BiodataRepository : IBiodataRepository
{
    private readonly AppDbContext _db;
    public BiodataRepository(AppDbContext db) => _db = db;

    private IQueryable<BiodataEntity> WithRelations() =>
        _db.Biodata
            .Include(b => b.User)
            .Include(b => b.Pendidikan)
            .Include(b => b.Pelatihan)
            .Include(b => b.Pekerjaan);

    public async Task<BiodataEntity?> FindByUserIdAsync(int userId) =>
        await WithRelations().FirstOrDefaultAsync(b => b.UserId == userId);

    public async Task<BiodataEntity?> FindByIdAsync(int id) =>
        await WithRelations().FirstOrDefaultAsync(b => b.Id == id);

    public async Task<BiodataEntity?> FindByNoKtpAsync(string noKtp) =>
        await _db.Biodata.FirstOrDefaultAsync(b => b.NoKtp == noKtp);

    public async Task<BiodataEntity> CreateAsync(BiodataEntity biodata)
    {
        biodata.CreatedAt = DateTime.Now;
        biodata.UpdatedAt = DateTime.Now;
        _db.Biodata.Add(biodata);
        await _db.SaveChangesAsync();
        return (await FindByIdAsync(biodata.Id))!;
    }

    public async Task<BiodataEntity> UpdateAsync(BiodataEntity biodata)
    {
        biodata.UpdatedAt = DateTime.Now;
        _db.Biodata.Update(biodata);
        await _db.SaveChangesAsync();
        return (await FindByIdAsync(biodata.Id))!;
    }

    public async Task DeleteAsync(int id)
    {
        var biodata = await _db.Biodata.FindAsync(id);
        if (biodata is not null)
        {
            _db.Biodata.Remove(biodata);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<PagedResult<BiodataEntity>> FindAllAsync(
        string? search, string? searchBy, int page, int limit)
    {
        var query = _db.Biodata
            .Include(b => b.User)
            .Include(b => b.Pendidikan)
            .Include(b => b.Pelatihan)
            .Include(b => b.Pekerjaan)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search) && !string.IsNullOrWhiteSpace(searchBy))
        {
            query = searchBy switch
            {
                "nama"    => query.Where(b => b.Nama.Contains(search)),
                "posisi"  => query.Where(b => b.PosisiDilamar.Contains(search)),
                "jenjang" => query.Where(b => b.Pendidikan.Any(p => p.Jenjang.Contains(search))),
                _         => query
            };
        }

        var total = await query.CountAsync();
        var data = await query
            .OrderByDescending(b => b.CreatedAt)
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync();

        return new PagedResult<BiodataEntity>
        {
            Data = data,
            Total = total,
            Page = page,
            Limit = limit
        };
    }
}
