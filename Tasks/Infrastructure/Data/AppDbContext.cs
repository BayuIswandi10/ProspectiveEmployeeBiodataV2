using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<BiodataEntity> Biodata => Set<BiodataEntity>();
    public DbSet<PendidikanEntity> Pendidikan => Set<PendidikanEntity>();
    public DbSet<PelatihanEntity> Pelatihan => Set<PelatihanEntity>();
    public DbSet<PekerjaanEntity> Pekerjaan => Set<PekerjaanEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Users
        modelBuilder.Entity<UserEntity>(e =>
        {
            e.ToTable("users", t => t.HasCheckConstraint("CK_Users_Role", "role IN ('user', 'admin')"));
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.Email).HasColumnName("email").HasMaxLength(255).IsRequired();
            e.Property(x => x.Password).HasColumnName("password").HasMaxLength(255).IsRequired();
            e.Property(x => x.Role).HasColumnName("role").HasMaxLength(10).HasDefaultValue("user");
            e.Property(x => x.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("GETDATE()");
            e.HasIndex(x => x.Email).IsUnique();
        });

        // Biodata
        modelBuilder.Entity<BiodataEntity>(e =>
        {
            e.ToTable("biodata", t => t.HasCheckConstraint("CK_Biodata_Penghasilan", "penghasilan_diharapkan > 0"));
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.UserId).HasColumnName("user_id");
            e.Property(x => x.PosisiDilamar).HasColumnName("posisi_dilamar").HasMaxLength(100);
            e.Property(x => x.Nama).HasColumnName("nama").HasMaxLength(100).IsRequired();
            e.Property(x => x.NoKtp).HasColumnName("no_ktp").HasMaxLength(20);
            e.Property(x => x.TempatLahir).HasColumnName("tempat_lahir").HasMaxLength(50);
            e.Property(x => x.TanggalLahir).HasColumnName("tanggal_lahir").HasColumnType("date");
            e.Property(x => x.JenisKelamin).HasColumnName("jenis_kelamin").HasMaxLength(15);
            e.Property(x => x.Agama).HasColumnName("agama").HasMaxLength(30);
            e.Property(x => x.GolonganDarah).HasColumnName("golongan_darah").HasMaxLength(5);
            e.Property(x => x.Status).HasColumnName("status").HasMaxLength(20);
            e.Property(x => x.AlamatKtp).HasColumnName("alamat_ktp").HasColumnType("nvarchar(max)");
            e.Property(x => x.AlamatTinggal).HasColumnName("alamat_tinggal").HasColumnType("nvarchar(max)");
            e.Property(x => x.Email).HasColumnName("email").HasMaxLength(100);
            e.Property(x => x.NoTelp).HasColumnName("no_telp").HasMaxLength(20);
            e.Property(x => x.OrangTerdekat).HasColumnName("orang_terdekat").HasMaxLength(100);
            e.Property(x => x.Skill).HasColumnName("skill").HasColumnType("nvarchar(max)");
            e.Property(x => x.BersediaDitempatkan).HasColumnName("bersedia_ditempatkan").HasDefaultValue(false);
            e.Property(x => x.PenghasilanDiharapkan).HasColumnName("penghasilan_diharapkan").HasColumnType("decimal(15,2)");
            e.Property(x => x.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("GETDATE()");
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("GETDATE()");

            // Since NoKtp can be null, we might not want a strict unique index, but EF will map it if we specify it. Let's keep it but it might cause issues if there are multiple nulls.
            e.HasIndex(x => x.NoKtp).IsUnique();
            e.HasIndex(x => x.UserId).IsUnique();

            e.HasOne(x => x.User)
                .WithOne(u => u.Biodata)
                .HasForeignKey<BiodataEntity>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Pendidikan
        modelBuilder.Entity<PendidikanEntity>(e =>
        {
            e.ToTable("pendidikan", t => t.HasCheckConstraint("CK_Pendidikan_Ipk", "ipk >= 0 AND ipk <= 4"));
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.BiodataId).HasColumnName("biodata_id");
            e.Property(x => x.Jenjang).HasColumnName("jenjang").HasMaxLength(50);
            e.Property(x => x.NamaInstitusi).HasColumnName("nama_institusi").HasMaxLength(100);
            e.Property(x => x.Jurusan).HasColumnName("jurusan").HasMaxLength(100);
            e.Property(x => x.TahunLulus).HasColumnName("tahun_lulus");
            e.Property(x => x.Ipk).HasColumnName("ipk").HasColumnType("decimal(4,2)");

            e.HasOne(x => x.Biodata)
                .WithMany(b => b.Pendidikan)
                .HasForeignKey(x => x.BiodataId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Pelatihan
        modelBuilder.Entity<PelatihanEntity>(e =>
        {
            e.ToTable("pelatihan");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.BiodataId).HasColumnName("biodata_id");
            e.Property(x => x.NamaKursus).HasColumnName("nama_kursus").HasMaxLength(100);
            e.Property(x => x.Sertifikat).HasColumnName("sertifikat").HasMaxLength(10);
            e.Property(x => x.Tahun).HasColumnName("tahun");

            e.HasOne(x => x.Biodata)
                .WithMany(b => b.Pelatihan)
                .HasForeignKey(x => x.BiodataId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Pekerjaan
        modelBuilder.Entity<PekerjaanEntity>(e =>
        {
            e.ToTable("pekerjaan", t => t.HasCheckConstraint("CK_Pekerjaan_Pendapatan", "pendapatan_terakhir >= 0"));
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(x => x.BiodataId).HasColumnName("biodata_id");
            e.Property(x => x.NamaPerusahaan).HasColumnName("nama_perusahaan").HasMaxLength(100);
            e.Property(x => x.PosisiTerakhir).HasColumnName("posisi_terakhir").HasMaxLength(100);
            e.Property(x => x.PendapatanTerakhir).HasColumnName("pendapatan_terakhir").HasColumnType("decimal(15,2)");
            e.Property(x => x.Tahun).HasColumnName("tahun");

            e.HasOne(x => x.Biodata)
                .WithMany(b => b.Pekerjaan)
                .HasForeignKey(x => x.BiodataId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
