using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class PendidikanRepository : IPendidikanRepository
{
    private readonly AppDbContext _db;
    public PendidikanRepository(AppDbContext db) => _db = db;

    public async Task CreateManyAsync(IEnumerable<PendidikanEntity> list)
    {
        _db.Pendidikan.AddRange(list);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteByBiodataIdAsync(int biodataId)
    {
        var items = _db.Pendidikan.Where(p => p.BiodataId == biodataId);
        _db.Pendidikan.RemoveRange(items);
        await _db.SaveChangesAsync();
    }
}
