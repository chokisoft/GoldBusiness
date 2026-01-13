using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoldBusiness.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTranslations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CuentaTranslation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CuentaId = table.Column<int>(type: "int", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CuentaTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CuentaTranslation_Cuenta_CuentaId",
                        column: x => x.CuentaId,
                        principalTable: "Cuenta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GrupoCuentaTranslation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GrupoCuentaId = table.Column<int>(type: "int", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrupoCuentaTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrupoCuentaTranslation_GrupoCuenta_GrupoCuentaId",
                        column: x => x.GrupoCuentaId,
                        principalTable: "GrupoCuenta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubGrupoCuentaTranslation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubGrupoCuentaId = table.Column<int>(type: "int", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubGrupoCuentaTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubGrupoCuentaTranslation_SubGrupoCuenta_SubGrupoCuentaId",
                        column: x => x.SubGrupoCuentaId,
                        principalTable: "SubGrupoCuenta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CuentaTranslation_CuentaId_Language",
                table: "CuentaTranslation",
                columns: new[] { "CuentaId", "Language" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GrupoCuentaTranslation_GrupoCuentaId_Language",
                table: "GrupoCuentaTranslation",
                columns: new[] { "GrupoCuentaId", "Language" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubGrupoCuentaTranslation_SubGrupoCuentaId_Language",
                table: "SubGrupoCuentaTranslation",
                columns: new[] { "SubGrupoCuentaId", "Language" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CuentaTranslation");

            migrationBuilder.DropTable(
                name: "GrupoCuentaTranslation");

            migrationBuilder.DropTable(
                name: "SubGrupoCuentaTranslation");
        }
    }
}
