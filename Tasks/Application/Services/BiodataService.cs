using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Models;

namespace Application.Services;

public class BiodataService : IBiodataService
{
    private readonly IBiodataRepository _biodataRepo;
    private readonly IPendidikanRepository _pendidikanRepo;
    private readonly IPelatihanRepository _pelatihanRepo;
    private readonly IPekerjaanRepository _pekerjaanRepo;

    public BiodataService(
        IBiodataRepository biodataRepo,
        IPendidikanRepository pendidikanRepo,
        IPelatihanRepository pelatihanRepo,
        IPekerjaanRepository pekerjaanRepo)
    {
        _biodataRepo = biodataRepo;
        _pendidikanRepo = pendidikanRepo;
        _pelatihanRepo = pelatihanRepo;
        _pekerjaanRepo = pekerjaanRepo;
    }

    public async Task<BiodataResponse> GetMyBiodataAsync(int userId)
    {
        var biodata = await _biodataRepo.FindByUserIdAsync(userId)
            ?? throw new KeyNotFoundException("Biodata belum diisi");
        return MapToResponse(biodata);
    }

    public async Task<BiodataResponse> CreateBiodataAsync(int userId, BiodataRequest request)
    {
        // Check existing biodata
        var existingByUser = await _biodataRepo.FindByUserIdAsync(userId);
        if (existingByUser is not null)
            throw new InvalidOperationException("Biodata sudah pernah diisi");

        // KTP uniqueness check
        if (!string.IsNullOrWhiteSpace(request.NoKtp))
        {
            var existingKtp = await _biodataRepo.FindByNoKtpAsync(request.NoKtp);
            if (existingKtp is not null)
                throw new InvalidOperationException("Nomor KTP sudah digunakan oleh kandidat lain");
        }

        var entity = MapToEntity(request, userId);
        var created = await _biodataRepo.CreateAsync(entity);

        // Create child records
        if (request.Pendidikan.Count > 0)
            await _pendidikanRepo.CreateManyAsync(request.Pendidikan.Select(p => new PendidikanEntity
            {
                BiodataId = created.Id, Jenjang = p.Jenjang, NamaInstitusi = p.NamaInstitusi,
                Jurusan = p.Jurusan, TahunLulus = p.TahunLulus, Ipk = p.Ipk
            }));

        if (request.Pelatihan.Count > 0)
            await _pelatihanRepo.CreateManyAsync(request.Pelatihan.Select(p => new PelatihanEntity
            {
                BiodataId = created.Id, NamaKursus = p.NamaKursus, Sertifikat = p.Sertifikat ? "Ada" : "Tidak", Tahun = p.Tahun
            }));

        if (request.Pekerjaan.Count > 0)
            await _pekerjaanRepo.CreateManyAsync(request.Pekerjaan.Select(p => new PekerjaanEntity
            {
                BiodataId = created.Id, NamaPerusahaan = p.NamaPerusahaan,
                PosisiTerakhir = p.PosisiTerakhir, PendapatanTerakhir = p.PendapatanTerakhir, Tahun = p.Tahun
            }));

        var result = await _biodataRepo.FindByUserIdAsync(userId);
        return MapToResponse(result!);
    }

    public async Task<BiodataResponse> UpdateBiodataAsync(
        int id, int userId, string userRole, BiodataRequest request)
    {
        var existing = await _biodataRepo.FindByIdAsync(id)
            ?? throw new KeyNotFoundException("Biodata tidak ditemukan");

        if (userRole != "admin" && existing.UserId != userId)
            throw new UnauthorizedAccessException("Tidak diizinkan mengubah biodata ini");

        // KTP uniqueness check (skip own record)
        if (!string.IsNullOrWhiteSpace(request.NoKtp))
        {
            var existingKtp = await _biodataRepo.FindByNoKtpAsync(request.NoKtp);
            if (existingKtp is not null && existingKtp.Id != id)
                throw new InvalidOperationException("Nomor KTP sudah digunakan oleh kandidat lain");
        }

        // Update main biodata fields
        existing.PosisiDilamar = request.PosisiDilamar;
        existing.Nama = request.Nama;
        existing.NoKtp = request.NoKtp;
        existing.TempatLahir = request.TempatLahir;
        existing.TanggalLahir = request.TanggalLahir;
        existing.JenisKelamin = request.JenisKelamin;
        existing.Agama = request.Agama;
        existing.GolonganDarah = request.GolonganDarah;
        existing.Status = request.Status;
        existing.AlamatKtp = request.AlamatKtp;
        existing.AlamatTinggal = request.AlamatTinggal;
        existing.Email = request.Email;
        existing.NoTelp = request.NoTelp;
        existing.OrangTerdekat = request.OrangTerdekat;
        existing.Skill = request.Skill;
        existing.BersediaDitempatkan = request.BersediaDitempatkan;
        existing.PenghasilanDiharapkan = request.PenghasilanDiharapkan;

        await _biodataRepo.UpdateAsync(existing);

        // Replace child tables
        await _pendidikanRepo.DeleteByBiodataIdAsync(id);
        if (request.Pendidikan.Count > 0)
            await _pendidikanRepo.CreateManyAsync(request.Pendidikan.Select(p => new PendidikanEntity
            {
                BiodataId = id, Jenjang = p.Jenjang, NamaInstitusi = p.NamaInstitusi,
                Jurusan = p.Jurusan, TahunLulus = p.TahunLulus, Ipk = p.Ipk
            }));

        await _pelatihanRepo.DeleteByBiodataIdAsync(id);
        if (request.Pelatihan.Count > 0)
            await _pelatihanRepo.CreateManyAsync(request.Pelatihan.Select(p => new PelatihanEntity
            {
                BiodataId = id, NamaKursus = p.NamaKursus, Sertifikat = p.Sertifikat ? "Ada" : "Tidak", Tahun = p.Tahun
            }));

        await _pekerjaanRepo.DeleteByBiodataIdAsync(id);
        if (request.Pekerjaan.Count > 0)
            await _pekerjaanRepo.CreateManyAsync(request.Pekerjaan.Select(p => new PekerjaanEntity
            {
                BiodataId = id, NamaPerusahaan = p.NamaPerusahaan,
                PosisiTerakhir = p.PosisiTerakhir, PendapatanTerakhir = p.PendapatanTerakhir, Tahun = p.Tahun
            }));

        var result = await _biodataRepo.FindByIdAsync(id);
        return MapToResponse(result!);
    }

    public async Task DeleteBiodataAsync(int id, int userId, string userRole)
    {
        var existing = await _biodataRepo.FindByIdAsync(id)
            ?? throw new KeyNotFoundException("Biodata tidak ditemukan");

        if (userRole != "admin" && existing.UserId != userId)
            throw new UnauthorizedAccessException("Tidak diizinkan menghapus biodata ini");

        await _biodataRepo.DeleteAsync(id);
    }

    // Mappers
    private static BiodataEntity MapToEntity(BiodataRequest r, int userId) => new()
    {
        UserId = userId, PosisiDilamar = r.PosisiDilamar, Nama = r.Nama, NoKtp = r.NoKtp,
        TempatLahir = r.TempatLahir, TanggalLahir = r.TanggalLahir, JenisKelamin = r.JenisKelamin,
        Agama = r.Agama, GolonganDarah = r.GolonganDarah, Status = r.Status, AlamatKtp = r.AlamatKtp,
        AlamatTinggal = r.AlamatTinggal, Email = r.Email, NoTelp = r.NoTelp, OrangTerdekat = r.OrangTerdekat,
        Skill = r.Skill, BersediaDitempatkan = r.BersediaDitempatkan, PenghasilanDiharapkan = r.PenghasilanDiharapkan
    };

    public static BiodataResponse MapToResponse(BiodataEntity b) => new()
    {
        Id = b.Id, UserId = b.UserId, PosisiDilamar = b.PosisiDilamar, Nama = b.Nama, NoKtp = b.NoKtp,
        TempatLahir = b.TempatLahir, TanggalLahir = b.TanggalLahir ?? default, JenisKelamin = b.JenisKelamin,
        Agama = b.Agama, GolonganDarah = b.GolonganDarah, Status = b.Status, AlamatKtp = b.AlamatKtp,
        AlamatTinggal = b.AlamatTinggal, Email = b.Email, NoTelp = b.NoTelp, OrangTerdekat = b.OrangTerdekat,
        Skill = b.Skill, BersediaDitempatkan = b.BersediaDitempatkan, PenghasilanDiharapkan = b.PenghasilanDiharapkan ?? 0m,
        CreatedAt = b.CreatedAt, UpdatedAt = b.UpdatedAt,
        User = b.User is null ? null : new UserDto { Id = b.User.Id, Email = b.User.Email, Role = b.User.Role },
        Pendidikan = b.Pendidikan.Select(p => new PendidikanResponse
        {
            Id = p.Id, BiodataId = p.BiodataId, Jenjang = p.Jenjang,
            NamaInstitusi = p.NamaInstitusi, Jurusan = p.Jurusan, TahunLulus = p.TahunLulus ?? 0, Ipk = p.Ipk
        }).ToList(),
        Pelatihan = b.Pelatihan.Select(p => new PelatihanResponse
        {
            Id = p.Id, BiodataId = p.BiodataId, NamaKursus = p.NamaKursus,
            Sertifikat = p.Sertifikat == "Ada", Tahun = p.Tahun ?? 0
        }).ToList(),
        Pekerjaan = b.Pekerjaan.Select(p => new PekerjaanResponse
        {
            Id = p.Id, BiodataId = p.BiodataId, NamaPerusahaan = p.NamaPerusahaan,
            PosisiTerakhir = p.PosisiTerakhir, PendapatanTerakhir = p.PendapatanTerakhir ?? 0m, Tahun = p.Tahun ?? 0
        }).ToList()
    };
}
