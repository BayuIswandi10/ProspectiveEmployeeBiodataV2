namespace Domain.Models;

public class BiodataRequest
{
    public string PosisiDilamar { get; set; } = string.Empty;
    public string Nama { get; set; } = string.Empty;
    public string NoKtp { get; set; } = string.Empty;
    public string TempatLahir { get; set; } = string.Empty;
    public DateTime TanggalLahir { get; set; }
    public string JenisKelamin { get; set; } = string.Empty;
    public string Agama { get; set; } = string.Empty;
    public string? GolonganDarah { get; set; }
    public string Status { get; set; } = string.Empty;
    public string AlamatKtp { get; set; } = string.Empty;
    public string AlamatTinggal { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string NoTelp { get; set; } = string.Empty;
    public string OrangTerdekat { get; set; } = string.Empty;
    public string? Skill { get; set; }
    public bool BersediaDitempatkan { get; set; }
    public decimal PenghasilanDiharapkan { get; set; }

    public List<PendidikanRequest> Pendidikan { get; set; } = new();
    public List<PelatihanRequest> Pelatihan { get; set; } = new();
    public List<PekerjaanRequest> Pekerjaan { get; set; } = new();
}

public class BiodataResponse
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string PosisiDilamar { get; set; } = string.Empty;
    public string Nama { get; set; } = string.Empty;
    public string NoKtp { get; set; } = string.Empty;
    public string TempatLahir { get; set; } = string.Empty;
    public DateTime TanggalLahir { get; set; }
    public string JenisKelamin { get; set; } = string.Empty;
    public string Agama { get; set; } = string.Empty;
    public string? GolonganDarah { get; set; }
    public string Status { get; set; } = string.Empty;
    public string AlamatKtp { get; set; } = string.Empty;
    public string AlamatTinggal { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string NoTelp { get; set; } = string.Empty;
    public string OrangTerdekat { get; set; } = string.Empty;
    public string? Skill { get; set; }
    public bool BersediaDitempatkan { get; set; }
    public decimal PenghasilanDiharapkan { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public UserDto? User { get; set; }

    public List<PendidikanResponse> Pendidikan { get; set; } = new();
    public List<PelatihanResponse> Pelatihan { get; set; } = new();
    public List<PekerjaanResponse> Pekerjaan { get; set; } = new();
}

// Pendidikan
public class PendidikanRequest
{
    public string Jenjang { get; set; } = string.Empty;
    public string NamaInstitusi { get; set; } = string.Empty;
    public string Jurusan { get; set; } = string.Empty;
    public int TahunLulus { get; set; }
    public decimal? Ipk { get; set; }
}

public class PendidikanResponse
{
    public int Id { get; set; }
    public int BiodataId { get; set; }
    public string Jenjang { get; set; } = string.Empty;
    public string NamaInstitusi { get; set; } = string.Empty;
    public string Jurusan { get; set; } = string.Empty;
    public int TahunLulus { get; set; }
    public decimal? Ipk { get; set; }
}

// Pelatihan
public class PelatihanRequest
{
    public string NamaKursus { get; set; } = string.Empty;
    public bool Sertifikat { get; set; }
    public int Tahun { get; set; }
}

public class PelatihanResponse
{
    public int Id { get; set; }
    public int BiodataId { get; set; }
    public string NamaKursus { get; set; } = string.Empty;
    public bool Sertifikat { get; set; }
    public int Tahun { get; set; }
}

// Pekerjaan
public class PekerjaanRequest
{
    public string NamaPerusahaan { get; set; } = string.Empty;
    public string PosisiTerakhir { get; set; } = string.Empty;
    public decimal PendapatanTerakhir { get; set; }
    public int Tahun { get; set; }
}

public class PekerjaanResponse
{
    public int Id { get; set; }
    public int BiodataId { get; set; }
    public string NamaPerusahaan { get; set; } = string.Empty;
    public string PosisiTerakhir { get; set; } = string.Empty;
    public decimal PendapatanTerakhir { get; set; }
    public int Tahun { get; set; }
}
