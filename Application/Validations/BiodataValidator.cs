using Domain.Models;
using FluentValidation;

namespace Application.Validations;

public class BiodataRequestValidator : AbstractValidator<BiodataRequest>
{
    public BiodataRequestValidator()
    {
        RuleFor(x => x.PosisiDilamar).NotEmpty().WithMessage("Posisi Dilamar wajib diisi");
        
        RuleFor(x => x.Nama)
            .NotEmpty().WithMessage("Nama wajib diisi")
            .MinimumLength(3).WithMessage("Nama minimal 3 karakter")
            .MaximumLength(100).WithMessage("Nama maksimal 100 karakter");

        RuleFor(x => x.NoKtp)
            .NotEmpty().WithMessage("Nomor KTP wajib diisi")
            .Matches(@"^\d{16}$").WithMessage("Nomor KTP harus terdiri dari 16 digit angka");

        RuleFor(x => x.TempatLahir).NotEmpty().WithMessage("Tempat Lahir wajib diisi");
        
        RuleFor(x => x.TanggalLahir)
            .NotEmpty().WithMessage("Tanggal Lahir wajib diisi")
            .LessThanOrEqualTo(DateTime.Today).WithMessage("Tanggal Lahir tidak boleh melebihi tanggal saat ini");

        RuleFor(x => x.JenisKelamin).NotEmpty().WithMessage("Jenis Kelamin wajib diisi");
        RuleFor(x => x.Agama).NotEmpty().WithMessage("Agama wajib diisi");
        RuleFor(x => x.Status).NotEmpty().WithMessage("Status wajib diisi");
        RuleFor(x => x.AlamatKtp).NotEmpty().WithMessage("Alamat KTP wajib diisi");
        RuleFor(x => x.AlamatTinggal).NotEmpty().WithMessage("Alamat Tinggal wajib diisi");
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email wajib diisi")
            .EmailAddress().WithMessage("Format email tidak valid");

        RuleFor(x => x.NoTelp)
            .NotEmpty().WithMessage("Nomor Telepon wajib diisi")
            .Matches(@"^\d+$").WithMessage("Nomor Telepon hanya boleh berisi angka")
            .MinimumLength(10).WithMessage("Nomor Telepon minimal 10 digit")
            .MaximumLength(15).WithMessage("Nomor Telepon maksimal 15 digit");

        RuleFor(x => x.OrangTerdekat).NotEmpty().WithMessage("Orang Terdekat wajib diisi");
        RuleFor(x => x.Skill).NotEmpty().WithMessage("Skill wajib diisi");
        
        RuleFor(x => x.BersediaDitempatkan).NotNull().WithMessage("Bersedia Ditempatkan wajib diisi");
        
        RuleFor(x => x.PenghasilanDiharapkan)
            .NotEmpty().WithMessage("Penghasilan Diharapkan wajib diisi")
            .GreaterThan(0).WithMessage("Penghasilan Diharapkan harus lebih besar dari 0");

        RuleFor(x => x.Pendidikan)
            .NotEmpty().WithMessage("Minimal harus memiliki 1 data pendidikan");

        RuleForEach(x => x.Pendidikan).SetValidator(new PendidikanRequestValidator());
        RuleForEach(x => x.Pelatihan).SetValidator(new PelatihanRequestValidator());
        RuleForEach(x => x.Pekerjaan).SetValidator(new PekerjaanRequestValidator());
    }
}

public class PendidikanRequestValidator : AbstractValidator<PendidikanRequest>
{
    public PendidikanRequestValidator()
    {
        RuleFor(x => x.Jenjang).NotEmpty().WithMessage("Jenjang wajib diisi");
        RuleFor(x => x.NamaInstitusi).NotEmpty().WithMessage("Nama Institusi wajib diisi");
        RuleFor(x => x.Jurusan).NotEmpty().WithMessage("Jurusan wajib diisi");
        
        RuleFor(x => x.TahunLulus)
            .NotEmpty().WithMessage("Tahun Lulus wajib diisi")
            .LessThanOrEqualTo(DateTime.Now.Year).WithMessage("Tahun Lulus tidak boleh lebih besar dari tahun berjalan");

        RuleFor(x => x.Ipk)
            .NotEmpty().WithMessage("IPK wajib diisi")
            .InclusiveBetween(0, 4).WithMessage("IPK harus berada pada rentang 0 sampai 4");
    }
}

public class PelatihanRequestValidator : AbstractValidator<PelatihanRequest>
{
    public PelatihanRequestValidator()
    {
        RuleFor(x => x.NamaKursus).NotEmpty().WithMessage("Nama Kursus wajib diisi");
        
        RuleFor(x => x.Tahun)
            .NotEmpty().WithMessage("Tahun wajib diisi")
            .LessThanOrEqualTo(DateTime.Now.Year).WithMessage("Tahun tidak boleh lebih besar dari tahun berjalan");
    }
}

public class PekerjaanRequestValidator : AbstractValidator<PekerjaanRequest>
{
    public PekerjaanRequestValidator()
    {
        RuleFor(x => x.NamaPerusahaan).NotEmpty().WithMessage("Nama Perusahaan wajib diisi");
        RuleFor(x => x.PosisiTerakhir).NotEmpty().WithMessage("Posisi Terakhir wajib diisi");
        
        RuleFor(x => x.Tahun)
            .NotEmpty().WithMessage("Tahun wajib diisi")
            .LessThanOrEqualTo(DateTime.Now.Year).WithMessage("Tahun tidak boleh lebih besar dari tahun berjalan");

        RuleFor(x => x.PendapatanTerakhir)
            .NotEmpty().WithMessage("Pendapatan Terakhir wajib diisi")
            .GreaterThanOrEqualTo(0).WithMessage("Pendapatan Terakhir tidak boleh bernilai negatif");
    }
}
