using Domain.Entities;

namespace Domain.Interfaces;

public interface IPelatihanRepository
{
    Task CreateManyAsync(IEnumerable<PelatihanEntity> list);
    Task DeleteByBiodataIdAsync(int biodataId);
}
