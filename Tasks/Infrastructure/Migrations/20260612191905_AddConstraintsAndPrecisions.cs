using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddConstraintsAndPrecisions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    role = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "USER"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.CheckConstraint("CK_Users_Role", "role IN ('USER', 'ADMIN')");
                });

            migrationBuilder.CreateTable(
                name: "biodata",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    posisi_dilamar = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    nama = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    no_ktp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    tempat_lahir = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    tanggal_lahir = table.Column<DateTime>(type: "date", nullable: true),
                    jenis_kelamin = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    agama = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    golongan_darah = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    alamat_ktp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    alamat_tinggal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    no_telp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    orang_terdekat = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    skill = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    bersedia_ditempatkan = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    penghasilan_diharapkan = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_biodata", x => x.id);
                    table.CheckConstraint("CK_Biodata_Penghasilan", "penghasilan_diharapkan > 0");
                    table.ForeignKey(
                        name: "FK_biodata_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pekerjaan",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    biodata_id = table.Column<int>(type: "int", nullable: false),
                    nama_perusahaan = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    posisi_terakhir = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    pendapatan_terakhir = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    tahun = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pekerjaan", x => x.id);
                    table.CheckConstraint("CK_Pekerjaan_Pendapatan", "pendapatan_terakhir >= 0");
                    table.ForeignKey(
                        name: "FK_pekerjaan_biodata_biodata_id",
                        column: x => x.biodata_id,
                        principalTable: "biodata",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pelatihan",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    biodata_id = table.Column<int>(type: "int", nullable: false),
                    nama_kursus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    sertifikat = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    tahun = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pelatihan", x => x.id);
                    table.ForeignKey(
                        name: "FK_pelatihan_biodata_biodata_id",
                        column: x => x.biodata_id,
                        principalTable: "biodata",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pendidikan",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    biodata_id = table.Column<int>(type: "int", nullable: false),
                    jenjang = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    nama_institusi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    jurusan = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    tahun_lulus = table.Column<int>(type: "int", nullable: true),
                    ipk = table.Column<decimal>(type: "decimal(4,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pendidikan", x => x.id);
                    table.CheckConstraint("CK_Pendidikan_Ipk", "ipk >= 0 AND ipk <= 4");
                    table.ForeignKey(
                        name: "FK_pendidikan_biodata_biodata_id",
                        column: x => x.biodata_id,
                        principalTable: "biodata",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_biodata_no_ktp",
                table: "biodata",
                column: "no_ktp",
                unique: true,
                filter: "[no_ktp] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_biodata_user_id",
                table: "biodata",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_pekerjaan_biodata_id",
                table: "pekerjaan",
                column: "biodata_id");

            migrationBuilder.CreateIndex(
                name: "IX_pelatihan_biodata_id",
                table: "pelatihan",
                column: "biodata_id");

            migrationBuilder.CreateIndex(
                name: "IX_pendidikan_biodata_id",
                table: "pendidikan",
                column: "biodata_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pekerjaan");

            migrationBuilder.DropTable(
                name: "pelatihan");

            migrationBuilder.DropTable(
                name: "pendidikan");

            migrationBuilder.DropTable(
                name: "biodata");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
