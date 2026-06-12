namespace Domain.Entities;

public class BiodataEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? PosisiDilamar { get; set; }
    public string Nama { get; set; } = string.Empty;
    public string? NoKtp { get; set; }
    public string? TempatLahir { get; set; }
    public DateTime? TanggalLahir { get; set; }
    public string? JenisKelamin { get; set; }
    public string? Agama { get; set; }
    public string? GolonganDarah { get; set; }
    public string? Status { get; set; }
    public string? AlamatKtp { get; set; }
    public string? AlamatTinggal { get; set; }
    public string? Email { get; set; }
    public string? NoTelp { get; set; }
    public string? OrangTerdekat { get; set; }
    public string? Skill { get; set; }
    public bool BersediaDitempatkan { get; set; }
    public decimal? PenghasilanDiharapkan { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation
    public UserEntity? User { get; set; }
    public ICollection<PendidikanEntity> Pendidikan { get; set; } = new List<PendidikanEntity>();
    public ICollection<PelatihanEntity> Pelatihan { get; set; } = new List<PelatihanEntity>();
    public ICollection<PekerjaanEntity> Pekerjaan { get; set; } = new List<PekerjaanEntity>();
}
