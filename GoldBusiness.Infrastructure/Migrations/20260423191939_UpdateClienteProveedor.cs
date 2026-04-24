using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoldBusiness.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateClienteProveedor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email1",
                table: "Proveedor");

            migrationBuilder.DropColumn(
                name: "Fax1",
                table: "Proveedor");

            migrationBuilder.DropColumn(
                name: "Fax2",
                table: "Proveedor");

            migrationBuilder.DropColumn(
                name: "Nif",
                table: "Proveedor");

            migrationBuilder.DropColumn(
                name: "Telefono1",
                table: "Proveedor");

            migrationBuilder.DropColumn(
                name: "Email1",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "Fax1",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "Fax2",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "Nif",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "Telefono1",
                table: "Cliente");

            migrationBuilder.RenameColumn(
                name: "Telefono2",
                table: "Proveedor",
                newName: "Telefono");

            migrationBuilder.RenameColumn(
                name: "Iva",
                table: "Proveedor",
                newName: "TasaIva");

            migrationBuilder.RenameColumn(
                name: "Email2",
                table: "Proveedor",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "Telefono2",
                table: "Cliente",
                newName: "Telefono");

            migrationBuilder.RenameColumn(
                name: "Iva",
                table: "Cliente",
                newName: "TasaIva");

            migrationBuilder.RenameColumn(
                name: "Email2",
                table: "Cliente",
                newName: "Email");

            migrationBuilder.AddColumn<string>(
                name: "IdentificadorFiscal",
                table: "Proveedor",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IdentificadorFiscal",
                table: "Cliente",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdentificadorFiscal",
                table: "Proveedor");

            migrationBuilder.DropColumn(
                name: "IdentificadorFiscal",
                table: "Cliente");

            migrationBuilder.RenameColumn(
                name: "Telefono",
                table: "Proveedor",
                newName: "Telefono2");

            migrationBuilder.RenameColumn(
                name: "TasaIva",
                table: "Proveedor",
                newName: "Iva");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Proveedor",
                newName: "Email2");

            migrationBuilder.RenameColumn(
                name: "Telefono",
                table: "Cliente",
                newName: "Telefono2");

            migrationBuilder.RenameColumn(
                name: "TasaIva",
                table: "Cliente",
                newName: "Iva");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Cliente",
                newName: "Email2");

            migrationBuilder.AddColumn<string>(
                name: "Email1",
                table: "Proveedor",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Fax1",
                table: "Proveedor",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Fax2",
                table: "Proveedor",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Nif",
                table: "Proveedor",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Telefono1",
                table: "Proveedor",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email1",
                table: "Cliente",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Fax1",
                table: "Cliente",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Fax2",
                table: "Cliente",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Nif",
                table: "Cliente",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Telefono1",
                table: "Cliente",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
