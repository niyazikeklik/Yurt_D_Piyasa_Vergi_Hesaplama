using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinansCore.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedStudentEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hisseler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Sembol = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hisseler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Alislar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Tarih = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Fiyat = table.Column<decimal>(type: "TEXT", nullable: false),
                    Adet = table.Column<decimal>(type: "TEXT", nullable: false),
                    HisseId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alislar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alislar_Hisseler_HisseId",
                        column: x => x.HisseId,
                        principalTable: "Hisseler",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Satislar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Tarih = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Fiyat = table.Column<decimal>(type: "TEXT", nullable: false),
                    Adet = table.Column<decimal>(type: "TEXT", nullable: false),
                    HisseId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Satislar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Satislar_Hisseler_HisseId",
                        column: x => x.HisseId,
                        principalTable: "Hisseler",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alislar_HisseId",
                table: "Alislar",
                column: "HisseId");

            migrationBuilder.CreateIndex(
                name: "IX_Satislar_HisseId",
                table: "Satislar",
                column: "HisseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alislar");

            migrationBuilder.DropTable(
                name: "Satislar");

            migrationBuilder.DropTable(
                name: "Hisseler");
        }
    }
}
