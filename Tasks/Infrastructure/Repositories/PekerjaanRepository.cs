using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class PekerjaanRepository : IPekerjaanRepository
{
    private readonly AppDbContext _db;
    public PekerjaanRepository(AppDbContext db) => _db = db;

    public async Task CreateManyAsync(IEnumerable<PekerjaanEntity> list)
    {
        _db.Pekerjaan.AddRange(list);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteByBiodataIdAsync(int biodataId)
    {
        var items = _db.Pekerjaan.Where(p => p.BiodataId == biodataId);
        _db.Pekerjaan.RemoveRange(items);
        await _db.SaveChangesAsync();
    }
}
