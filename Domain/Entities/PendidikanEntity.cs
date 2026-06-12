namespace Domain.Entities;

public class PendidikanEntity
{
    public int Id { get; set; }
    public int BiodataId { get; set; }
    public string? Jenjang { get; set; }
    public string? NamaInstitusi { get; set; }
    public string? Jurusan { get; set; }
    public int? TahunLulus { get; set; }
    public decimal? Ipk { get; set; }

    // Navigation
    public BiodataEntity? Biodata { get; set; }
}
