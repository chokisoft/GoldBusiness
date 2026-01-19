using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoldBusiness.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGrupo_SubGrupo_Cuenta : Migration
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
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrupoCuenta", x => x.Id);
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
                name: "SubGrupoCuenta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    GrupoCuentaId = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Deudora = table.Column<bool>(type: "bit", nullable: false),
                    Cancelado = table.Column<bool>(type: "bit", nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubGrupoCuenta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubGrupoCuenta_GrupoCuenta",
                        column: x => x.GrupoCuentaId,
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
                name: "IX_CuentaTranslation_CuentaId_Language",
                table: "CuentaTranslation",
                columns: new[] { "CuentaId", "Language" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GrupoCuenta",
                table: "GrupoCuenta",
                columns: new[] { "Descripcion", "Cancelado" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GrupoCuentaTranslation_GrupoCuentaId_Language",
                table: "GrupoCuentaTranslation",
                columns: new[] { "GrupoCuentaId", "Language" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubGrupoCuenta",
                table: "SubGrupoCuenta",
                columns: new[] { "Codigo", "GrupoCuentaId", "Cancelado" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubGrupoCuenta_GrupoCuentaId",
                table: "SubGrupoCuenta",
                column: "GrupoCuentaId");

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

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Cuenta");

            migrationBuilder.DropTable(
                name: "SubGrupoCuenta");

            migrationBuilder.DropTable(
                name: "GrupoCuenta");
        }
    }
}
