using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoldBusiness.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Grupo_SubGrupo_Cuenta_Update20250112 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GrupoCuenta",
                table: "SubGrupoCuenta",
                newName: "GrupoCuentaId");

            migrationBuilder.RenameIndex(
                name: "IX_SubGrupoCuenta_GrupoCuenta",
                table: "SubGrupoCuenta",
                newName: "IX_SubGrupoCuenta_GrupoCuentaId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaHoraModificado",
                table: "SubGrupoCuenta",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaHoraModificado",
                table: "GrupoCuenta",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GrupoCuentaId",
                table: "SubGrupoCuenta",
                newName: "GrupoCuenta");

            migrationBuilder.RenameIndex(
                name: "IX_SubGrupoCuenta_GrupoCuentaId",
                table: "SubGrupoCuenta",
                newName: "IX_SubGrupoCuenta_GrupoCuenta");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaHoraModificado",
                table: "SubGrupoCuenta",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaHoraModificado",
                table: "GrupoCuenta",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);
        }
    }
}
