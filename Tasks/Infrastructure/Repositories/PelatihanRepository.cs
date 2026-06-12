using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class PelatihanRepository : IPelatihanRepository
{
    private readonly AppDbContext _db;
    public PelatihanRepository(AppDbContext db) => _db = db;

    public async Task CreateManyAsync(IEnumerable<PelatihanEntity> list)
    {
        _db.Pelatihan.AddRange(list);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteByBiodataIdAsync(int biodataId)
    {
        var items = _db.Pelatihan.Where(p => p.BiodataId == biodataId);
        _db.Pelatihan.RemoveRange(items);
        await _db.SaveChangesAsync();
    }
}
