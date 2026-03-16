using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoldBusiness.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddArreglosTablas15032026 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CuentaCobrarPagar_Cliente",
                table: "CuentaCobrarPagar");

            migrationBuilder.DropForeignKey(
                name: "FK_CuentaCobrarPagar_Proveedor",
                table: "CuentaCobrarPagar");

            migrationBuilder.DropColumn(
                name: "CodPostal",
                table: "Proveedor");

            migrationBuilder.DropColumn(
                name: "Municipio",
                table: "Proveedor");

            migrationBuilder.DropColumn(
                name: "Provincia",
                table: "Proveedor");

            migrationBuilder.DropColumn(
                name: "CodPostal",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "Municipio",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "Provincia",
                table: "Cliente");

            migrationBuilder.AddColumn<int>(
                name: "CodigoPostalId",
                table: "Proveedor",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MunicipioId",
                table: "Proveedor",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaisId",
                table: "Proveedor",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProvinciaId",
                table: "Proveedor",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CodigoPostalId",
                table: "Establecimiento",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Direccion",
                table: "Establecimiento",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "MunicipioId",
                table: "Establecimiento",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaisId",
                table: "Establecimiento",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProvinciaId",
                table: "Establecimiento",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telefono",
                table: "Establecimiento",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CodigoPostalId",
                table: "Cliente",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MunicipioId",
                table: "Cliente",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaisId",
                table: "Cliente",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProvinciaId",
                table: "Cliente",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Proveedor_CodigoPostalId",
                table: "Proveedor",
                column: "CodigoPostalId");

            migrationBuilder.CreateIndex(
                name: "IX_Proveedor_MunicipioId",
                table: "Proveedor",
                column: "MunicipioId");

            migrationBuilder.CreateIndex(
                name: "IX_Proveedor_PaisId",
                table: "Proveedor",
                column: "PaisId");

            migrationBuilder.CreateIndex(
                name: "IX_Proveedor_ProvinciaId",
                table: "Proveedor",
                column: "ProvinciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Establecimiento_CodigoPostalId",
                table: "Establecimiento",
                column: "CodigoPostalId");

            migrationBuilder.CreateIndex(
                name: "IX_Establecimiento_MunicipioId",
                table: "Establecimiento",
                column: "MunicipioId");

            migrationBuilder.CreateIndex(
                name: "IX_Establecimiento_PaisId",
                table: "Establecimiento",
                column: "PaisId");

            migrationBuilder.CreateIndex(
                name: "IX_Establecimiento_ProvinciaId",
                table: "Establecimiento",
                column: "ProvinciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_CodigoPostalId",
                table: "Cliente",
                column: "CodigoPostalId");

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_MunicipioId",
                table: "Cliente",
                column: "MunicipioId");

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_PaisId",
                table: "Cliente",
                column: "PaisId");

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_ProvinciaId",
                table: "Cliente",
                column: "ProvinciaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cliente_CodigoPostal",
                table: "Cliente",
                column: "CodigoPostalId",
                principalTable: "CodigoPostal",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Cliente_Municipio",
                table: "Cliente",
                column: "MunicipioId",
                principalTable: "Municipio",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Cliente_Pais",
                table: "Cliente",
                column: "PaisId",
                principalTable: "Pais",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Cliente_Provincia",
                table: "Cliente",
                column: "ProvinciaId",
                principalTable: "Provincia",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_CuentaCobrarPagar_Cliente_ClienteId",
                table: "CuentaCobrarPagar",
                column: "ClienteId",
                principalTable: "Cliente",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CuentaCobrarPagar_Proveedor_ProveedorId",
                table: "CuentaCobrarPagar",
                column: "ProveedorId",
                principalTable: "Proveedor",
                principalColumn: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Proveedor_CodigoPostal",
                table: "Proveedor",
                column: "CodigoPostalId",
                principalTable: "CodigoPostal",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Proveedor_Municipio",
                table: "Proveedor",
                column: "MunicipioId",
                principalTable: "Municipio",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Proveedor_Pais",
                table: "Proveedor",
                column: "PaisId",
                principalTable: "Pais",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Proveedor_Provincia",
                table: "Proveedor",
                column: "ProvinciaId",
                principalTable: "Provincia",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cliente_CodigoPostal",
                table: "Cliente");

            migrationBuilder.DropForeignKey(
                name: "FK_Cliente_Municipio",
                table: "Cliente");

            migrationBuilder.DropForeignKey(
                name: "FK_Cliente_Pais",
                table: "Cliente");

            migrationBuilder.DropForeignKey(
                name: "FK_Cliente_Provincia",
                table: "Cliente");

            migrationBuilder.DropForeignKey(
                name: "FK_CuentaCobrarPagar_Cliente_ClienteId",
                table: "CuentaCobrarPagar");

            migrationBuilder.DropForeignKey(
                name: "FK_CuentaCobrarPagar_Proveedor_ProveedorId",
                table: "CuentaCobrarPagar");

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

            migrationBuilder.DropForeignKey(
                name: "FK_Proveedor_CodigoPostal",
                table: "Proveedor");

            migrationBuilder.DropForeignKey(
                name: "FK_Proveedor_Municipio",
                table: "Proveedor");

            migrationBuilder.DropForeignKey(
                name: "FK_Proveedor_Pais",
                table: "Proveedor");

            migrationBuilder.DropForeignKey(
                name: "FK_Proveedor_Provincia",
                table: "Proveedor");

            migrationBuilder.DropIndex(
                name: "IX_Proveedor_CodigoPostalId",
                table: "Proveedor");

            migrationBuilder.DropIndex(
                name: "IX_Proveedor_MunicipioId",
                table: "Proveedor");

            migrationBuilder.DropIndex(
                name: "IX_Proveedor_PaisId",
                table: "Proveedor");

            migrationBuilder.DropIndex(
                name: "IX_Proveedor_ProvinciaId",
                table: "Proveedor");

            migrationBuilder.DropIndex(
                name: "IX_Establecimiento_CodigoPostalId",
                table: "Establecimiento");

            migrationBuilder.DropIndex(
                name: "IX_Establecimiento_MunicipioId",
                table: "Establecimiento");

            migrationBuilder.DropIndex(
                name: "IX_Establecimiento_PaisId",
                table: "Establecimiento");

            migrationBuilder.DropIndex(
                name: "IX_Establecimiento_ProvinciaId",
                table: "Establecimiento");

            migrationBuilder.DropIndex(
                name: "IX_Cliente_CodigoPostalId",
                table: "Cliente");

            migrationBuilder.DropIndex(
                name: "IX_Cliente_MunicipioId",
                table: "Cliente");

            migrationBuilder.DropIndex(
                name: "IX_Cliente_PaisId",
                table: "Cliente");

            migrationBuilder.DropIndex(
                name: "IX_Cliente_ProvinciaId",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "CodigoPostalId",
                table: "Proveedor");

            migrationBuilder.DropColumn(
                name: "MunicipioId",
                table: "Proveedor");

            migrationBuilder.DropColumn(
                name: "PaisId",
                table: "Proveedor");

            migrationBuilder.DropColumn(
                name: "ProvinciaId",
                table: "Proveedor");

            migrationBuilder.DropColumn(
                name: "CodigoPostalId",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "Direccion",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "MunicipioId",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "PaisId",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "ProvinciaId",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "Telefono",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "CodigoPostalId",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "MunicipioId",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "PaisId",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "ProvinciaId",
                table: "Cliente");

            migrationBuilder.AddColumn<string>(
                name: "CodPostal",
                table: "Proveedor",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Municipio",
                table: "Proveedor",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Provincia",
                table: "Proveedor",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CodPostal",
                table: "Cliente",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Municipio",
                table: "Cliente",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Provincia",
                table: "Cliente",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_CuentaCobrarPagar_Cliente",
                table: "CuentaCobrarPagar",
                column: "ClienteId",
                principalTable: "Cliente",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_CuentaCobrarPagar_Proveedor",
                table: "CuentaCobrarPagar",
                column: "ProveedorId",
                principalTable: "Proveedor",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
