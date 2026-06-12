using Domain.Entities;

namespace Domain.Interfaces;

public interface IPekerjaanRepository
{
    Task CreateManyAsync(IEnumerable<PekerjaanEntity> list);
    Task DeleteByBiodataIdAsync(int biodataId);
}
