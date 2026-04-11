using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoldBusiness.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPropertyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentificadorFiscal",
                table: "SystemConfiguration",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IvaInternacional",
                table: "SystemConfiguration",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RegimenFiscal",
                table: "SystemConfiguration",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "RegistradaIva",
                table: "SystemConfiguration",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "TasaIvaDefecto",
                table: "SystemConfiguration",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "TipoIdentificadorFiscal",
                table: "SystemConfiguration",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CodigoPaisIso",
                table: "Proveedor",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ExentoIva",
                table: "Proveedor",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Extranjero",
                table: "Proveedor",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "InversionSujetoPasivo",
                table: "Proveedor",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RegimenFiscal",
                table: "Proveedor",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TipoIdentificadorFiscal",
                table: "Proveedor",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ValidarIdentificadorFiscal",
                table: "Proveedor",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CodigoPaisIso",
                table: "Cliente",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ExentoIva",
                table: "Cliente",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Extranjero",
                table: "Cliente",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "InversionSujetoPasivo",
                table: "Cliente",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RegimenFiscal",
                table: "Cliente",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TipoIdentificadorFiscal",
                table: "Cliente",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ValidarIdentificadorFiscal",
                table: "Cliente",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdentificadorFiscal",
                table: "SystemConfiguration");

            migrationBuilder.DropColumn(
                name: "IvaInternacional",
                table: "SystemConfiguration");

            migrationBuilder.DropColumn(
                name: "RegimenFiscal",
                table: "SystemConfiguration");

            migrationBuilder.DropColumn(
                name: "RegistradaIva",
                table: "SystemConfiguration");

            migrationBuilder.DropColumn(
                name: "TasaIvaDefecto",
                table: "SystemConfiguration");

            migrationBuilder.DropColumn(
                name: "TipoIdentificadorFiscal",
                table: "SystemConfiguration");

            migrationBuilder.DropColumn(
                name: "CodigoPaisIso",
                table: "Proveedor");

            migrationBuilder.DropColumn(
                name: "ExentoIva",
                table: "Proveedor");

            migrationBuilder.DropColumn(
                name: "Extranjero",
                table: "Proveedor");

            migrationBuilder.DropColumn(
                name: "InversionSujetoPasivo",
                table: "Proveedor");

            migrationBuilder.DropColumn(
                name: "RegimenFiscal",
                table: "Proveedor");

            migrationBuilder.DropColumn(
                name: "TipoIdentificadorFiscal",
                table: "Proveedor");

            migrationBuilder.DropColumn(
                name: "ValidarIdentificadorFiscal",
                table: "Proveedor");

            migrationBuilder.DropColumn(
                name: "CodigoPaisIso",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "ExentoIva",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "Extranjero",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "InversionSujetoPasivo",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "RegimenFiscal",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "TipoIdentificadorFiscal",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "ValidarIdentificadorFiscal",
                table: "Cliente");
        }
    }
}
