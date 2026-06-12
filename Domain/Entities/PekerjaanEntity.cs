namespace Domain.Entities;

public class PekerjaanEntity
{
    public int Id { get; set; }
    public int BiodataId { get; set; }
    public string? NamaPerusahaan { get; set; }
    public string? PosisiTerakhir { get; set; }
    public decimal? PendapatanTerakhir { get; set; }
    public int? Tahun { get; set; }

    // Navigation
    public BiodataEntity? Biodata { get; set; }
}
