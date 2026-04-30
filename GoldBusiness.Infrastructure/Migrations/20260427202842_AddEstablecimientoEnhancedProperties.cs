using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoldBusiness.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEstablecimientoEnhancedProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Establecimiento_CodigoPostal",
                table: "Establecimiento");

            migrationBuilder.DropForeignKey(
                name: "FK_Establecimiento_Municipio",
                table: "Establecimiento");

            migrationBuilder.DropForeignKey(
                name: "FK_Establecimiento_Pais",
                table: "Establecimiento");

            migrationBuilder.DropForeignKey(
                name: "FK_Establecimiento_Provincia",
                table: "Establecimiento");

            migrationBuilder.AddColumn<int>(
                name: "AlmacenPrincipalId",
                table: "Establecimiento",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AreaM2",
                table: "Establecimiento",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CapacidadAlmacenM3",
                table: "Establecimiento",
                type: "int",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CapacidadEmpleados",
                table: "Establecimiento",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CargoResponsable",
                table: "Establecimiento",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CentroCostos",
                table: "Establecimiento",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CertificadoDigital",
                table: "Establecimiento",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodigoAPI",
                table: "Establecimiento",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodigoExterno",
                table: "Establecimiento",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodigoImpuestoLocal",
                table: "Establecimiento",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodigoOdoo",
                table: "Establecimiento",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodigoSAP",
                table: "Establecimiento",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CuentaContableId",
                table: "Establecimiento",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DireccionAlternativa",
                table: "Establecimiento",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailCompras",
                table: "Establecimiento",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailFacturacion",
                table: "Establecimiento",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailGeneral",
                table: "Establecimiento",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailResponsable",
                table: "Establecimiento",
                type: "nvarchar(256)",
                maxLength: 256,
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

            migrationBuilder.AddColumn<string>(
                name: "Fax",
                table: "Establecimiento",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaApertura",
                table: "Establecimiento",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCierre",
                table: "Establecimiento",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "HorarioAperturaD",
                table: "Establecimiento",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "HorarioAperturaLV",
                table: "Establecimiento",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "HorarioAperturaS",
                table: "Establecimiento",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "HorarioCierreD",
                table: "Establecimiento",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "HorarioCierreLV",
                table: "Establecimiento",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "HorarioCierreS",
                table: "Establecimiento",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HorarioEspecial",
                table: "Establecimiento",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentificadorFiscalLocal",
                table: "Establecimiento",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Latitud",
                table: "Establecimiento",
                type: "decimal(10,7)",
                precision: 10,
                scale: 7,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LicenciaComercial",
                table: "Establecimiento",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitud",
                table: "Establecimiento",
                type: "decimal(10,7)",
                precision: 10,
                scale: 7,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MetadataIntegracion",
                table: "Establecimiento",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MonedaId",
                table: "Establecimiento",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NombreResponsable",
                table: "Establecimiento",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "OperativoActualmente",
                table: "Establecimiento",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "PermiteCompras",
                table: "Establecimiento",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "PermiteEcommerce",
                table: "Establecimiento",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PermiteInventario",
                table: "Establecimiento",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "PermitePOS",
                table: "Establecimiento",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PermiteVentas",
                table: "Establecimiento",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "PrefijoFactura",
                table: "Establecimiento",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrefijoOrdenCompra",
                table: "Establecimiento",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Referencia",
                table: "Establecimiento",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegistroMercantil",
                table: "Establecimiento",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RequiereFacturacionElectronica",
                table: "Establecimiento",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SecuenciaActual",
                table: "Establecimiento",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SitioWeb",
                table: "Establecimiento",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TasaIvaLocal",
                table: "Establecimiento",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TelefonoAlternativo",
                table: "Establecimiento",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TelefonoResponsable",
                table: "Establecimiento",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Tipo",
                table: "Establecimiento",
                type: "int",
                nullable: false,
                defaultValue: 2);

            migrationBuilder.AddColumn<string>(
                name: "ZonaHoraria",
                table: "Establecimiento",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "UTC");

            migrationBuilder.CreateIndex(
                name: "IX_Establecimiento_CuentaContableId",
                table: "Establecimiento",
                column: "CuentaContableId");

            migrationBuilder.CreateIndex(
                name: "IX_Establecimiento_EstablecimientoMatrizId",
                table: "Establecimiento",
                column: "EstablecimientoMatrizId");

            migrationBuilder.CreateIndex(
                name: "IX_Establecimiento_MonedaId",
                table: "Establecimiento",
                column: "MonedaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Establecimiento_CodigoPostal",
                table: "Establecimiento",
                column: "CodigoPostalId",
                principalTable: "CodigoPostal",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Establecimiento_CuentaContable",
                table: "Establecimiento",
                column: "CuentaContableId",
                principalTable: "Cuenta",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Establecimiento_EstablecimientoMatriz",
                table: "Establecimiento",
                column: "EstablecimientoMatrizId",
                principalTable: "Establecimiento",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Establecimiento_Moneda",
                table: "Establecimiento",
                column: "MonedaId",
                principalTable: "Moneda",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Establecimiento_Municipio",
                table: "Establecimiento",
                column: "MunicipioId",
                principalTable: "Municipio",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Establecimiento_Pais",
                table: "Establecimiento",
                column: "PaisId",
                principalTable: "Pais",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Establecimiento_Provincia",
                table: "Establecimiento",
                column: "ProvinciaId",
                principalTable: "Provincia",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Establecimiento_CodigoPostal",
                table: "Establecimiento");

            migrationBuilder.DropForeignKey(
                name: "FK_Establecimiento_CuentaContable",
                table: "Establecimiento");

            migrationBuilder.DropForeignKey(
                name: "FK_Establecimiento_EstablecimientoMatriz",
                table: "Establecimiento");

            migrationBuilder.DropForeignKey(
                name: "FK_Establecimiento_Moneda",
                table: "Establecimiento");

            migrationBuilder.DropForeignKey(
                name: "FK_Establecimiento_Municipio",
                table: "Establecimiento");

            migrationBuilder.DropForeignKey(
                name: "FK_Establecimiento_Pais",
                table: "Establecimiento");

            migrationBuilder.DropForeignKey(
                name: "FK_Establecimiento_Provincia",
                table: "Establecimiento");

            migrationBuilder.DropIndex(
                name: "IX_Establecimiento_CuentaContableId",
                table: "Establecimiento");

            migrationBuilder.DropIndex(
                name: "IX_Establecimiento_EstablecimientoMatrizId",
                table: "Establecimiento");

            migrationBuilder.DropIndex(
                name: "IX_Establecimiento_MonedaId",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "AlmacenPrincipalId",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "AreaM2",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "CapacidadAlmacenM3",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "CapacidadEmpleados",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "CargoResponsable",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "CentroCostos",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "CertificadoDigital",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "CodigoAPI",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "CodigoExterno",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "CodigoImpuestoLocal",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "CodigoOdoo",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "CodigoSAP",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "CuentaContableId",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "DireccionAlternativa",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "EmailCompras",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "EmailFacturacion",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "EmailGeneral",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "EmailResponsable",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "EsMatriz",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "EstablecimientoMatrizId",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "Fax",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "FechaApertura",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "FechaCierre",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "HorarioAperturaD",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "HorarioAperturaLV",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "HorarioAperturaS",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "HorarioCierreD",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "HorarioCierreLV",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "HorarioCierreS",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "HorarioEspecial",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "IdentificadorFiscalLocal",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "Latitud",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "LicenciaComercial",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "Longitud",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "MetadataIntegracion",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "MonedaId",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "NombreResponsable",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "OperativoActualmente",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "PermiteCompras",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "PermiteEcommerce",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "PermiteInventario",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "PermitePOS",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "PermiteVentas",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "PrefijoFactura",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "PrefijoOrdenCompra",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "Referencia",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "RegistroMercantil",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "RequiereFacturacionElectronica",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "SecuenciaActual",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "SitioWeb",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "TasaIvaLocal",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "TelefonoAlternativo",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "TelefonoResponsable",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "ZonaHoraria",
                table: "Establecimiento");

            migrationBuilder.AddForeignKey(
                name: "FK_Establecimiento_CodigoPostal",
                table: "Establecimiento",
                column: "CodigoPostalId",
                principalTable: "CodigoPostal",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Establecimiento_Municipio",
                table: "Establecimiento",
                column: "MunicipioId",
                principalTable: "Municipio",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Establecimiento_Pais",
                table: "Establecimiento",
                column: "PaisId",
                principalTable: "Pais",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Establecimiento_Provincia",
                table: "Establecimiento",
                column: "ProvinciaId",
                principalTable: "Provincia",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
