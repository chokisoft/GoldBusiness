using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoldBusiness.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ERPModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ══════════════════════════════════════════════════════════════════════════════
            // ELIMINACIÓN DE CAMPOS REDUNDANTES EN ESTABLECIMIENTO
            // ══════════════════════════════════════════════════════════════════════════════
            // Razón: SystemConfiguration YA ES la entidad raíz/matriz del negocio.
            // Los establecimientos SON TODOS dependientes del negocio.
            // No se necesita jerarquía Establecimiento → Establecimiento.
            // ══════════════════════════════════════════════════════════════════════════════

            // 1. Eliminar Foreign Key de auto-referencia
            migrationBuilder.DropForeignKey(
                name: "FK_Establecimiento_EstablecimientoMatriz",
                table: "Establecimiento");

            // 2. Eliminar columnas redundantes
            migrationBuilder.DropColumn(
                name: "CodigoExterno",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "EsMatriz",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "EstablecimientoMatrizId",
                table: "Establecimiento");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revertir cambios (restaurar columnas eliminadas)
            migrationBuilder.AddColumn<string>(
                name: "CodigoExterno",
                table: "Establecimiento",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EsMatriz",
                table: "Establecimiento",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "EstablecimientoMatrizId",
                table: "Establecimiento",
                type: "int",
                nullable: true);

            // Restaurar Foreign Key de auto-referencia
            migrationBuilder.AddForeignKey(
                name: "FK_Establecimiento_EstablecimientoMatriz",
                table: "Establecimiento",
                column: "EstablecimientoMatrizId",
                principalTable: "Establecimiento",
                principalColumn: "Id");
        }
    }
}
