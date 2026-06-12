namespace Domain.Entities;

public class PelatihanEntity
{
    public int Id { get; set; }
    public int BiodataId { get; set; }
    public string? NamaKursus { get; set; }
    public string? Sertifikat { get; set; }
    public int? Tahun { get; set; }

    // Navigation
    public BiodataEntity? Biodata { get; set; }
}
