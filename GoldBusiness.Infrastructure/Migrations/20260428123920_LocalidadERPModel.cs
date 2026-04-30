using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoldBusiness.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LocalidadERPModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Almacen",
                table: "Localidad");

            migrationBuilder.AddColumn<bool>(
                name: "PermiteAjustes",
                table: "Localidad",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "PermiteCompras",
                table: "Localidad",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PermiteTransferencias",
                table: "Localidad",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "PermiteVentas",
                table: "Localidad",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RequiereControlLotes",
                table: "Localidad",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RequiereNumerosSerie",
                table: "Localidad",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Tipo",
                table: "Localidad",
                type: "int",
                nullable: false,
                defaultValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PermiteAjustes",
                table: "Localidad");

            migrationBuilder.DropColumn(
                name: "PermiteCompras",
                table: "Localidad");

            migrationBuilder.DropColumn(
                name: "PermiteTransferencias",
                table: "Localidad");

            migrationBuilder.DropColumn(
                name: "PermiteVentas",
                table: "Localidad");

            migrationBuilder.DropColumn(
                name: "RequiereControlLotes",
                table: "Localidad");

            migrationBuilder.DropColumn(
                name: "RequiereNumerosSerie",
                table: "Localidad");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Localidad");

            migrationBuilder.AddColumn<bool>(
                name: "Almacen",
                table: "Localidad",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
