using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoldBusiness.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Grupo_SubGrupo_Cuenta_Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GrupoCuenta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Cancelado = table.Column<bool>(type: "bit", nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrupoCuenta", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubGrupoCuenta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    GrupoCuenta = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Deudora = table.Column<bool>(type: "bit", nullable: false),
                    Cancelado = table.Column<bool>(type: "bit", nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubGrupoCuenta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubGrupoCuenta_GrupoCuenta",
                        column: x => x.GrupoCuenta,
                        principalTable: "GrupoCuenta",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Cuenta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    SubGrupoCuentaId = table.Column<int>(type: "int", nullable: false),
                    Cancelado = table.Column<bool>(type: "bit", nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cuenta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cuenta_SubGrupoCuenta",
                        column: x => x.SubGrupoCuentaId,
                        principalTable: "SubGrupoCuenta",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cuenta",
                table: "Cuenta",
                columns: new[] { "Codigo", "SubGrupoCuentaId", "Cancelado" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cuenta_SubGrupo",
                table: "Cuenta",
                column: "SubGrupoCuentaId");

            migrationBuilder.CreateIndex(
                name: "IX_GrupoCuenta",
                table: "GrupoCuenta",
                columns: new[] { "Descripcion", "Cancelado" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubGrupoCuenta",
                table: "SubGrupoCuenta",
                columns: new[] { "Codigo", "GrupoCuenta", "Cancelado" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubGrupoCuenta_GrupoCuenta",
                table: "SubGrupoCuenta",
                column: "GrupoCuenta");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cuenta");

            migrationBuilder.DropTable(
                name: "SubGrupoCuenta");

            migrationBuilder.DropTable(
                name: "GrupoCuenta");
        }
    }
}
