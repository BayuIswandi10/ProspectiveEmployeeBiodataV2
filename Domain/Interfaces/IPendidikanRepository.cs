using Domain.Entities;

namespace Domain.Interfaces;

public interface IPendidikanRepository
{
    Task CreateManyAsync(IEnumerable<PendidikanEntity> list);
    Task DeleteByBiodataIdAsync(int biodataId);
}
