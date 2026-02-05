using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoldBusiness.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Nif = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Iban = table.Column<string>(type: "nvarchar(27)", maxLength: 27, nullable: false),
                    BicoSwift = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Iva = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Municipio = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Provincia = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CodPostal = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Web = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Email1 = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Email2 = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Telefono1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Telefono2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Fax1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Fax2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Cancelado = table.Column<bool>(type: "bit", nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.Id);
                });

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
                name: "IdTurno",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime", nullable: false),
                    Cajero = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Inicio = table.Column<DateTime>(type: "datetime", nullable: false),
                    Fondo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Extraccion = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Cierre = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdTurno", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Linea",
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
                    table.PrimaryKey("PK_Linea", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Proveedor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Nif = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Iban = table.Column<string>(type: "nvarchar(27)", maxLength: 27, nullable: false),
                    BicoSwift = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Iva = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Municipio = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Provincia = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CodPostal = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Web = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Email1 = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Email2 = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Telefono1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Telefono2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Fax1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Fax2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Cancelado = table.Column<bool>(type: "bit", nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proveedor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transaccion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaccion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnidadMedida",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Cancelado = table.Column<bool>(type: "bit", nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnidadMedida", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClienteTranslation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClienteTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClienteTranslation_Cliente",
                        column: x => x.ClienteId,
                        principalTable: "Cliente",
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
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
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
                name: "CajaRegistradora",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdTurnoId = table.Column<int>(type: "int", nullable: false),
                    Mesa = table.Column<int>(type: "int", nullable: true),
                    Cerrado = table.Column<bool>(type: "bit", nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CajaRegistradora", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CajaRegistradora_IdTurno",
                        column: x => x.IdTurnoId,
                        principalTable: "IdTurno",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LineaTranslation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LineaId = table.Column<int>(type: "int", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineaTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LineaTranslation_Linea_LineaId",
                        column: x => x.LineaId,
                        principalTable: "Linea",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubLinea",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    LineaId = table.Column<int>(type: "int", nullable: false),
                    Cancelado = table.Column<bool>(type: "bit", nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubLinea", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubLinea_Linea",
                        column: x => x.LineaId,
                        principalTable: "Linea",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProveedorTranslation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProveedorId = table.Column<int>(type: "int", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProveedorTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProveedorTranslation_Proveedor",
                        column: x => x.ProveedorId,
                        principalTable: "Proveedor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransaccionTranslation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransaccionId = table.Column<int>(type: "int", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransaccionTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransaccionTranslation_Transaccion",
                        column: x => x.TransaccionId,
                        principalTable: "Transaccion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UnidadMedidaTranslation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnidadMedidaId = table.Column<int>(type: "int", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnidadMedidaTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnidadMedidaTranslation_UnidadMedida",
                        column: x => x.UnidadMedidaId,
                        principalTable: "UnidadMedida",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
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
                name: "SubLineaTranslation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubLineaId = table.Column<int>(type: "int", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubLineaTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubLineaTranslation_SubLinea_SubLineaId",
                        column: x => x.SubLineaId,
                        principalTable: "SubLinea",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConceptoAjuste",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CuentaId = table.Column<int>(type: "int", nullable: false),
                    Cancelado = table.Column<bool>(type: "bit", nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConceptoAjuste", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConceptoAjuste_Cuenta",
                        column: x => x.CuentaId,
                        principalTable: "Cuenta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CuentaTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CuentaTranslation_Cuenta_CuentaId",
                        column: x => x.CuentaId,
                        principalTable: "Cuenta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SystemConfiguration",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoSistema = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Licencia = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NombreNegocio = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Municipio = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Provincia = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    CodPostal = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Imagen = table.Column<byte[]>(type: "image", nullable: false),
                    Web = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CuentaPagarId = table.Column<int>(type: "int", nullable: false),
                    CuentaCobrarId = table.Column<int>(type: "int", nullable: false),
                    Caducidad = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemConfiguration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configuracion_CuentaCobrar",
                        column: x => x.CuentaCobrarId,
                        principalTable: "Cuenta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Configuracion_CuentaPagar",
                        column: x => x.CuentaPagarId,
                        principalTable: "Cuenta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConceptoAjusteTranslation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConceptoAjusteId = table.Column<int>(type: "int", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConceptoAjusteTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConceptoAjusteTranslation_ConceptoAjuste",
                        column: x => x.ConceptoAjusteId,
                        principalTable: "ConceptoAjuste",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Establecimiento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NegocioId = table.Column<int>(type: "int", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Cancelado = table.Column<bool>(type: "bit", nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Establecimiento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Establecimiento_Configuracion",
                        column: x => x.NegocioId,
                        principalTable: "SystemConfiguration",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SystemConfigurationTranslation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConfiguracionId = table.Column<int>(type: "int", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    NombreNegocio = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemConfigurationTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfiguracionTranslation_Configuracion",
                        column: x => x.ConfiguracionId,
                        principalTable: "SystemConfiguration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comprobante",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstablecimientoId = table.Column<int>(type: "int", nullable: false),
                    NoComprobante = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Automatico = table.Column<bool>(type: "bit", nullable: false),
                    Posteado = table.Column<bool>(type: "bit", nullable: false),
                    Cancelado = table.Column<bool>(type: "bit", nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comprobante", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comprobante_Establecimiento",
                        column: x => x.EstablecimientoId,
                        principalTable: "Establecimiento",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ComprobanteTemporal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstablecimientoId = table.Column<int>(type: "int", nullable: false),
                    CodigoTransaccion = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Transaccion = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    NoDocumento = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime", nullable: false),
                    Cuenta = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Departamento = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Inventario = table.Column<bool>(type: "bit", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Debito = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Credito = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Parcial = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprobanteTemporal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComprobanteTemporal_Establecimiento",
                        column: x => x.EstablecimientoId,
                        principalTable: "Establecimiento",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CuentaCobrarPagar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstablecimientoId = table.Column<int>(type: "int", nullable: false),
                    TransaccionId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime", nullable: false),
                    ProveedorId = table.Column<int>(type: "int", nullable: true),
                    ClienteId = table.Column<int>(type: "int", nullable: true),
                    NoPrimario = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NoDocumento = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Importe = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CuentaPagoEfectivoId = table.Column<int>(type: "int", nullable: true),
                    PagoEfectivoDepartamento = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PagoEfectivoImporte = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PagoEfectivoParcialMlc = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CuentaPagoElectronicoId = table.Column<int>(type: "int", nullable: true),
                    PagoElectronicoDepartamento = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PagoElectronicoImporte = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PagoElectronicoParcialMlc = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CuentaCobroEfectivoId = table.Column<int>(type: "int", nullable: true),
                    CobroEfectivoDepartamento = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CobroEfectivoImporte = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CobroEfectivoParcialMlc = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CuentaCobroElectronicoId = table.Column<int>(type: "int", nullable: true),
                    CobroElectronicoDepartamento = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CobroElectronicoImporte = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CobroElectronicoParcialMlc = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Contabilizada = table.Column<bool>(type: "bit", nullable: false),
                    Cancelado = table.Column<bool>(type: "bit", nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CuentaCobrarPagar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CuentaCobrarPagar_Cliente",
                        column: x => x.ClienteId,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_CuentaCobrarPagar_CuentaCobroEfectivo",
                        column: x => x.CuentaCobroEfectivoId,
                        principalTable: "Cuenta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CuentaCobrarPagar_CuentaCobroElectronico",
                        column: x => x.CuentaCobroElectronicoId,
                        principalTable: "Cuenta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CuentaCobrarPagar_CuentaPagoEfectivo",
                        column: x => x.CuentaPagoEfectivoId,
                        principalTable: "Cuenta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CuentaCobrarPagar_CuentaPagoElectronico",
                        column: x => x.CuentaPagoElectronicoId,
                        principalTable: "Cuenta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CuentaCobrarPagar_Establecimiento",
                        column: x => x.EstablecimientoId,
                        principalTable: "Establecimiento",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CuentaCobrarPagar_Proveedor",
                        column: x => x.ProveedorId,
                        principalTable: "Proveedor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_CuentaCobrarPagar_Transaccion",
                        column: x => x.TransaccionId,
                        principalTable: "Transaccion",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EstablecimientoTranslation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstablecimientoId = table.Column<int>(type: "int", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstablecimientoTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstablecimientoTranslation_Establecimiento",
                        column: x => x.EstablecimientoId,
                        principalTable: "Establecimiento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EstadoCuenta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstablecimientoId = table.Column<int>(type: "int", nullable: false),
                    CuentaId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime", nullable: false),
                    Debito = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Credito = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Saldo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Referencia = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoCuenta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstadoCuenta_Cuenta",
                        column: x => x.CuentaId,
                        principalTable: "Cuenta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EstadoCuenta_Establecimiento",
                        column: x => x.EstablecimientoId,
                        principalTable: "Establecimiento",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Localidad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstablecimientoId = table.Column<int>(type: "int", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Almacen = table.Column<bool>(type: "bit", nullable: false),
                    CuentaInventarioId = table.Column<int>(type: "int", nullable: false),
                    CuentaCostoId = table.Column<int>(type: "int", nullable: false),
                    CuentaVentaId = table.Column<int>(type: "int", nullable: false),
                    CuentaDevolucionId = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Cancelado = table.Column<bool>(type: "bit", nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Localidad", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Localidad_CuentaCosto",
                        column: x => x.CuentaCostoId,
                        principalTable: "Cuenta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Localidad_CuentaDevolucion",
                        column: x => x.CuentaDevolucionId,
                        principalTable: "Cuenta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Localidad_CuentaInventario",
                        column: x => x.CuentaInventarioId,
                        principalTable: "Cuenta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Localidad_CuentaVenta",
                        column: x => x.CuentaVentaId,
                        principalTable: "Cuenta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Localidad_Establecimiento",
                        column: x => x.EstablecimientoId,
                        principalTable: "Establecimiento",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Moneda",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstablecimientoId = table.Column<int>(type: "int", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Cancelado = table.Column<bool>(type: "bit", nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Moneda", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Moneda_Establecimiento",
                        column: x => x.EstablecimientoId,
                        principalTable: "Establecimiento",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OperacionesEncabezado",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstablecimientoId = table.Column<int>(type: "int", nullable: false),
                    TransaccionId = table.Column<int>(type: "int", nullable: false),
                    NoDocumento = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime", nullable: false),
                    ProveedorId = table.Column<int>(type: "int", nullable: true),
                    ClienteId = table.Column<int>(type: "int", nullable: true),
                    NoPrimario = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReferenciaId = table.Column<int>(type: "int", nullable: true),
                    ConceptoAjusteId = table.Column<int>(type: "int", nullable: true),
                    Concepto = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Efectivo = table.Column<bool>(type: "bit", nullable: false),
                    Contabilizada = table.Column<bool>(type: "bit", nullable: false),
                    Cancelado = table.Column<bool>(type: "bit", nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperacionesEncabezado", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperacionesEncabezado_Cliente",
                        column: x => x.ClienteId,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_OperacionesEncabezado_ConceptoAjuste",
                        column: x => x.ConceptoAjusteId,
                        principalTable: "ConceptoAjuste",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_OperacionesEncabezado_Establecimiento",
                        column: x => x.EstablecimientoId,
                        principalTable: "Establecimiento",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OperacionesEncabezado_Proveedor",
                        column: x => x.ProveedorId,
                        principalTable: "Proveedor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_OperacionesEncabezado_Transaccion",
                        column: x => x.TransaccionId,
                        principalTable: "Transaccion",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Producto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstablecimientoId = table.Column<int>(type: "int", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    UnidadMedidaId = table.Column<int>(type: "int", nullable: false),
                    ProveedorId = table.Column<int>(type: "int", nullable: false),
                    PrecioVenta = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecioCosto = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValueSql: "((0.00))"),
                    Iva = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValueSql: "((0.00))"),
                    CodigoReferencia = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StockMinimo = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValueSql: "((0.00))"),
                    Servicio = table.Column<bool>(type: "bit", nullable: false),
                    SubLineaId = table.Column<int>(type: "int", nullable: false),
                    Imagen = table.Column<byte[]>(type: "image", nullable: false),
                    Caracteristicas = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Cancelado = table.Column<bool>(type: "bit", nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Producto_Establecimiento",
                        column: x => x.EstablecimientoId,
                        principalTable: "Establecimiento",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Producto_Proveedor",
                        column: x => x.ProveedorId,
                        principalTable: "Proveedor",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Producto_SubLinea",
                        column: x => x.SubLineaId,
                        principalTable: "SubLinea",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Producto_UnidadMedida",
                        column: x => x.UnidadMedidaId,
                        principalTable: "UnidadMedida",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ComprobanteDetalle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComprobanteId = table.Column<int>(type: "int", nullable: false),
                    CuentaId = table.Column<int>(type: "int", nullable: false),
                    Departamento = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Debito = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Credito = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Parcial = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Nota = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Cancelado = table.Column<bool>(type: "bit", nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprobanteDetalle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComprobanteDetalle_Comprobante",
                        column: x => x.ComprobanteId,
                        principalTable: "Comprobante",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ComprobanteDetalle_Cuenta",
                        column: x => x.CuentaId,
                        principalTable: "Cuenta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ComprobanteTranslation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComprobanteId = table.Column<int>(type: "int", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprobanteTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComprobanteTranslation_Comprobante",
                        column: x => x.ComprobanteId,
                        principalTable: "Comprobante",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComprobanteTemporalTranslation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComprobanteTemporalId = table.Column<int>(type: "int", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprobanteTemporalTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComprobanteTemporalTranslation_ComprobanteTemporal",
                        column: x => x.ComprobanteTemporalId,
                        principalTable: "ComprobanteTemporal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LocalidadTranslation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocalidadId = table.Column<int>(type: "int", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalidadTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocalidadTranslation_Localidad",
                        column: x => x.LocalidadId,
                        principalTable: "Localidad",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MonedaTranslation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MonedaId = table.Column<int>(type: "int", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonedaTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonedaTranslation_Moneda",
                        column: x => x.MonedaId,
                        principalTable: "Moneda",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperacionesEncabezadoTranslation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperacionesEncabezadoId = table.Column<int>(type: "int", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Concepto = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperacionesEncabezadoTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperacionesEncabezadoTranslation_OperacionesEncabezado",
                        column: x => x.OperacionesEncabezadoId,
                        principalTable: "OperacionesEncabezado",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CajaRegistradoraDetalle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CajaRegistradoraId = table.Column<int>(type: "int", nullable: false),
                    LocalidadId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    Venta = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImporteVenta = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CajaRegistradoraDetalle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CajaRegistradoraDetalle_CajaRegistradora",
                        column: x => x.CajaRegistradoraId,
                        principalTable: "CajaRegistradora",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CajaRegistradoraDetalle_Localidad",
                        column: x => x.LocalidadId,
                        principalTable: "Localidad",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CajaRegistradoraDetalle_Producto",
                        column: x => x.ProductoId,
                        principalTable: "Producto",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FichaProducto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    LocalidadId = table.Column<int>(type: "int", nullable: false),
                    ComponenteId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    Cancelado = table.Column<bool>(type: "bit", nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FichaProducto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FichaProducto_Componente",
                        column: x => x.ComponenteId,
                        principalTable: "Producto",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FichaProducto_Localidad",
                        column: x => x.LocalidadId,
                        principalTable: "Localidad",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FichaProducto_Producto",
                        column: x => x.ProductoId,
                        principalTable: "Producto",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OperacionesDetalle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperacionesEncabezadoId = table.Column<int>(type: "int", nullable: false),
                    LocalidadId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<decimal>(type: "numeric(18,3)", nullable: false, defaultValueSql: "((0.00))"),
                    Costo = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValueSql: "((0.00))"),
                    ImporteCosto = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValueSql: "((0.00))"),
                    Venta = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValueSql: "((0.00))"),
                    ImporteVenta = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValueSql: "((0.00))"),
                    Existencia = table.Column<decimal>(type: "numeric(18,3)", nullable: false, defaultValueSql: "((0.00))"),
                    Cancelado = table.Column<bool>(type: "bit", nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperacionesDetalle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperacionesDetalle_Localidad",
                        column: x => x.LocalidadId,
                        principalTable: "Localidad",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OperacionesDetalle_OperacionesEncabezado",
                        column: x => x.OperacionesEncabezadoId,
                        principalTable: "OperacionesEncabezado",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OperacionesDetalle_Producto",
                        column: x => x.ProductoId,
                        principalTable: "Producto",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductoTranslation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Caracteristicas = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductoTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductoTranslation_Producto",
                        column: x => x.ProductoId,
                        principalTable: "Producto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Saldo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocalidadId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    Existencia = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Saldo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Saldo_Localidad",
                        column: x => x.LocalidadId,
                        principalTable: "Localidad",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Saldo_Producto",
                        column: x => x.ProductoId,
                        principalTable: "Producto",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SaldoAnterior",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocalidadId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    PrecioCosto = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValueSql: "((0.00))"),
                    Existencia = table.Column<decimal>(type: "numeric(18,3)", nullable: false),
                    ImporteCosto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaldoAnterior", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaldoAnterior_Localidad",
                        column: x => x.LocalidadId,
                        principalTable: "Localidad",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SaldoAnterior_Producto",
                        column: x => x.ProductoId,
                        principalTable: "Producto",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ComprobanteDetalleTranslation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComprobanteDetalleId = table.Column<int>(type: "int", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Nota = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprobanteDetalleTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComprobanteDetalleTranslation_ComprobanteDetalle",
                        column: x => x.ComprobanteDetalleId,
                        principalTable: "ComprobanteDetalle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ErroresVenta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperacionesDetalleId = table.Column<int>(type: "int", nullable: false),
                    LocalidadId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Costo = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ImporteCosto = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Servicio = table.Column<bool>(type: "bit", nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErroresVenta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ErroresVenta_Localidad",
                        column: x => x.LocalidadId,
                        principalTable: "Localidad",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ErroresVenta_OperacionesDetalle",
                        column: x => x.OperacionesDetalleId,
                        principalTable: "OperacionesDetalle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ErroresVenta_Producto",
                        column: x => x.ProductoId,
                        principalTable: "Producto",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OperacionesServicio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperacionesDetalleId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<decimal>(type: "decimal(18,3)", nullable: false, defaultValueSql: "((0.00))"),
                    Costo = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValueSql: "((0.00))"),
                    ImporteCosto = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValueSql: "((0.00))"),
                    Cancelado = table.Column<bool>(type: "bit", nullable: false),
                    CreadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraCreado = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificadoPor = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FechaHoraModificado = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperacionesServicio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperacionesServicio_OperacionesDetalle",
                        column: x => x.OperacionesDetalleId,
                        principalTable: "OperacionesDetalle",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OperacionesServicio_Producto",
                        column: x => x.ProductoId,
                        principalTable: "Producto",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CajaRegistradora_IdTurnoId",
                table: "CajaRegistradora",
                column: "IdTurnoId");

            migrationBuilder.CreateIndex(
                name: "IX_CajaRegistradoraDetalle_CajaRegistradoraId",
                table: "CajaRegistradoraDetalle",
                column: "CajaRegistradoraId");

            migrationBuilder.CreateIndex(
                name: "IX_CajaRegistradoraDetalle_LocalidadId",
                table: "CajaRegistradoraDetalle",
                column: "LocalidadId");

            migrationBuilder.CreateIndex(
                name: "IX_CajaRegistradoraDetalle_ProductoId",
                table: "CajaRegistradoraDetalle",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_Cliente",
                table: "Cliente",
                columns: new[] { "Codigo", "Cancelado" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClienteTranslation_ClienteId_Language",
                table: "ClienteTranslation",
                columns: new[] { "ClienteId", "Language" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comprobante_EstablecimientoId",
                table: "Comprobante",
                column: "EstablecimientoId");

            migrationBuilder.CreateIndex(
                name: "IX_ComprobanteDetalle_ComprobanteId",
                table: "ComprobanteDetalle",
                column: "ComprobanteId");

            migrationBuilder.CreateIndex(
                name: "IX_ComprobanteDetalle_CuentaId",
                table: "ComprobanteDetalle",
                column: "CuentaId");

            migrationBuilder.CreateIndex(
                name: "IX_ComprobanteDetalleTranslation_ComprobanteDetalleId_Language",
                table: "ComprobanteDetalleTranslation",
                columns: new[] { "ComprobanteDetalleId", "Language" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ComprobanteTemporal_EstablecimientoId",
                table: "ComprobanteTemporal",
                column: "EstablecimientoId");

            migrationBuilder.CreateIndex(
                name: "IX_ComprobanteTemporalTranslation_ComprobanteTemporalId_Language",
                table: "ComprobanteTemporalTranslation",
                columns: new[] { "ComprobanteTemporalId", "Language" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ComprobanteTranslation_ComprobanteId_Language",
                table: "ComprobanteTranslation",
                columns: new[] { "ComprobanteId", "Language" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConceptoAjuste_CuentaId",
                table: "ConceptoAjuste",
                column: "CuentaId");

            migrationBuilder.CreateIndex(
                name: "IX_ConceptoAjusteTranslation_ConceptoAjusteId_Language",
                table: "ConceptoAjusteTranslation",
                columns: new[] { "ConceptoAjusteId", "Language" },
                unique: true);

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
                name: "IX_CuentaCobrarPagar_ClienteId",
                table: "CuentaCobrarPagar",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_CuentaCobrarPagar_CuentaCobroEfectivoId",
                table: "CuentaCobrarPagar",
                column: "CuentaCobroEfectivoId");

            migrationBuilder.CreateIndex(
                name: "IX_CuentaCobrarPagar_CuentaCobroElectronicoId",
                table: "CuentaCobrarPagar",
                column: "CuentaCobroElectronicoId");

            migrationBuilder.CreateIndex(
                name: "IX_CuentaCobrarPagar_CuentaPagoEfectivoId",
                table: "CuentaCobrarPagar",
                column: "CuentaPagoEfectivoId");

            migrationBuilder.CreateIndex(
                name: "IX_CuentaCobrarPagar_CuentaPagoElectronicoId",
                table: "CuentaCobrarPagar",
                column: "CuentaPagoElectronicoId");

            migrationBuilder.CreateIndex(
                name: "IX_CuentaCobrarPagar_EstablecimientoId",
                table: "CuentaCobrarPagar",
                column: "EstablecimientoId");

            migrationBuilder.CreateIndex(
                name: "IX_CuentaCobrarPagar_ProveedorId",
                table: "CuentaCobrarPagar",
                column: "ProveedorId");

            migrationBuilder.CreateIndex(
                name: "IX_CuentaCobrarPagar_TransaccionId",
                table: "CuentaCobrarPagar",
                column: "TransaccionId");

            migrationBuilder.CreateIndex(
                name: "IX_CuentaTranslation_CuentaId_Language",
                table: "CuentaTranslation",
                columns: new[] { "CuentaId", "Language" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ErroresVenta_Localidad",
                table: "ErroresVenta",
                column: "LocalidadId");

            migrationBuilder.CreateIndex(
                name: "IX_ErroresVenta_OperacionesDetalle",
                table: "ErroresVenta",
                column: "OperacionesDetalleId");

            migrationBuilder.CreateIndex(
                name: "IX_ErroresVenta_Producto",
                table: "ErroresVenta",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_Establecimiento",
                table: "Establecimiento",
                columns: new[] { "NegocioId", "Codigo", "Cancelado" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EstablecimientoTranslation_EstablecimientoId_Language",
                table: "EstablecimientoTranslation",
                columns: new[] { "EstablecimientoId", "Language" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EstadoCuenta_CuentaId",
                table: "EstadoCuenta",
                column: "CuentaId");

            migrationBuilder.CreateIndex(
                name: "IX_EstadoCuenta_EstablecimientoId",
                table: "EstadoCuenta",
                column: "EstablecimientoId");

            migrationBuilder.CreateIndex(
                name: "IX_FichaProducto_ComponenteId",
                table: "FichaProducto",
                column: "ComponenteId");

            migrationBuilder.CreateIndex(
                name: "IX_FichaProducto_LocalidadId",
                table: "FichaProducto",
                column: "LocalidadId");

            migrationBuilder.CreateIndex(
                name: "IX_FichaProducto_ProductoId",
                table: "FichaProducto",
                column: "ProductoId");

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
                name: "IX_Linea",
                table: "Linea",
                columns: new[] { "Codigo", "Cancelado" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LineaTranslation_LineaId_Language",
                table: "LineaTranslation",
                columns: new[] { "LineaId", "Language" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Localidad",
                table: "Localidad",
                columns: new[] { "EstablecimientoId", "Codigo", "Cancelado" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Localidad_CuentaCostoId",
                table: "Localidad",
                column: "CuentaCostoId");

            migrationBuilder.CreateIndex(
                name: "IX_Localidad_CuentaDevolucionId",
                table: "Localidad",
                column: "CuentaDevolucionId");

            migrationBuilder.CreateIndex(
                name: "IX_Localidad_CuentaInventarioId",
                table: "Localidad",
                column: "CuentaInventarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Localidad_CuentaVentaId",
                table: "Localidad",
                column: "CuentaVentaId");

            migrationBuilder.CreateIndex(
                name: "IX_LocalidadTranslation_LocalidadId_Language",
                table: "LocalidadTranslation",
                columns: new[] { "LocalidadId", "Language" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Moneda_EstablecimientoId_Codigo_Cancelado",
                table: "Moneda",
                columns: new[] { "EstablecimientoId", "Codigo", "Cancelado" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MonedaTranslation_MonedaId_Language",
                table: "MonedaTranslation",
                columns: new[] { "MonedaId", "Language" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OperacionesDetalle_LocalidadId",
                table: "OperacionesDetalle",
                column: "LocalidadId");

            migrationBuilder.CreateIndex(
                name: "IX_OperacionesDetalle_OperacionesEncabezadoId",
                table: "OperacionesDetalle",
                column: "OperacionesEncabezadoId");

            migrationBuilder.CreateIndex(
                name: "IX_OperacionesDetalle_ProductoId",
                table: "OperacionesDetalle",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_OperacionesEncabezado_ClienteId",
                table: "OperacionesEncabezado",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_OperacionesEncabezado_ConceptoAjusteId",
                table: "OperacionesEncabezado",
                column: "ConceptoAjusteId");

            migrationBuilder.CreateIndex(
                name: "IX_OperacionesEncabezado_EstablecimientoId",
                table: "OperacionesEncabezado",
                column: "EstablecimientoId");

            migrationBuilder.CreateIndex(
                name: "IX_OperacionesEncabezado_ProveedorId",
                table: "OperacionesEncabezado",
                column: "ProveedorId");

            migrationBuilder.CreateIndex(
                name: "IX_OperacionesEncabezado_TransaccionId",
                table: "OperacionesEncabezado",
                column: "TransaccionId");

            migrationBuilder.CreateIndex(
                name: "IX_OperacionesEncabezadoTranslation_OperacionesEncabezadoId_Language",
                table: "OperacionesEncabezadoTranslation",
                columns: new[] { "OperacionesEncabezadoId", "Language" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OperacionesServicio_OperacionesDetalleId",
                table: "OperacionesServicio",
                column: "OperacionesDetalleId");

            migrationBuilder.CreateIndex(
                name: "IX_OperacionesServicio_ProductoId",
                table: "OperacionesServicio",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_Producto",
                table: "Producto",
                columns: new[] { "EstablecimientoId", "Codigo", "Cancelado" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Producto_ProveedorId",
                table: "Producto",
                column: "ProveedorId");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_SubLineaId",
                table: "Producto",
                column: "SubLineaId");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_UnidadMedidaId",
                table: "Producto",
                column: "UnidadMedidaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductoTranslation_ProductoId_Language",
                table: "ProductoTranslation",
                columns: new[] { "ProductoId", "Language" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Proveedor",
                table: "Proveedor",
                columns: new[] { "Codigo", "Cancelado" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProveedorTranslation_ProveedorId_Language",
                table: "ProveedorTranslation",
                columns: new[] { "ProveedorId", "Language" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Saldo_Localidad_Producto",
                table: "Saldo",
                columns: new[] { "LocalidadId", "ProductoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Saldo_LocalidadId",
                table: "Saldo",
                column: "LocalidadId");

            migrationBuilder.CreateIndex(
                name: "IX_Saldo_ProductoId",
                table: "Saldo",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_SaldoAnterior_Localidad_Producto_Fecha",
                table: "SaldoAnterior",
                columns: new[] { "LocalidadId", "ProductoId", "Fecha" });

            migrationBuilder.CreateIndex(
                name: "IX_SaldoAnterior_LocalidadId",
                table: "SaldoAnterior",
                column: "LocalidadId");

            migrationBuilder.CreateIndex(
                name: "IX_SaldoAnterior_ProductoId",
                table: "SaldoAnterior",
                column: "ProductoId");

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

            migrationBuilder.CreateIndex(
                name: "IX_SubLinea",
                table: "SubLinea",
                columns: new[] { "Codigo", "LineaId", "Cancelado" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubLinea_LineaId",
                table: "SubLinea",
                column: "LineaId");

            migrationBuilder.CreateIndex(
                name: "IX_SubLineaTranslation_SubLineaId_Language",
                table: "SubLineaTranslation",
                columns: new[] { "SubLineaId", "Language" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SystemConfiguration_CuentaCobrarId",
                table: "SystemConfiguration",
                column: "CuentaCobrarId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemConfiguration_CuentaPagarId",
                table: "SystemConfiguration",
                column: "CuentaPagarId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemConfigurationTranslation_ConfiguracionId_Language",
                table: "SystemConfigurationTranslation",
                columns: new[] { "ConfiguracionId", "Language" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransaccionTranslation_TransaccionId_Language",
                table: "TransaccionTranslation",
                columns: new[] { "TransaccionId", "Language" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UnidadMedida",
                table: "UnidadMedida",
                columns: new[] { "Codigo", "Cancelado" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UnidadMedidaTranslation_UnidadMedidaId_Language",
                table: "UnidadMedidaTranslation",
                columns: new[] { "UnidadMedidaId", "Language" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CajaRegistradoraDetalle");

            migrationBuilder.DropTable(
                name: "ClienteTranslation");

            migrationBuilder.DropTable(
                name: "ComprobanteDetalleTranslation");

            migrationBuilder.DropTable(
                name: "ComprobanteTemporalTranslation");

            migrationBuilder.DropTable(
                name: "ComprobanteTranslation");

            migrationBuilder.DropTable(
                name: "ConceptoAjusteTranslation");

            migrationBuilder.DropTable(
                name: "CuentaCobrarPagar");

            migrationBuilder.DropTable(
                name: "CuentaTranslation");

            migrationBuilder.DropTable(
                name: "ErroresVenta");

            migrationBuilder.DropTable(
                name: "EstablecimientoTranslation");

            migrationBuilder.DropTable(
                name: "EstadoCuenta");

            migrationBuilder.DropTable(
                name: "FichaProducto");

            migrationBuilder.DropTable(
                name: "GrupoCuentaTranslation");

            migrationBuilder.DropTable(
                name: "LineaTranslation");

            migrationBuilder.DropTable(
                name: "LocalidadTranslation");

            migrationBuilder.DropTable(
                name: "MonedaTranslation");

            migrationBuilder.DropTable(
                name: "OperacionesEncabezadoTranslation");

            migrationBuilder.DropTable(
                name: "OperacionesServicio");

            migrationBuilder.DropTable(
                name: "ProductoTranslation");

            migrationBuilder.DropTable(
                name: "ProveedorTranslation");

            migrationBuilder.DropTable(
                name: "Saldo");

            migrationBuilder.DropTable(
                name: "SaldoAnterior");

            migrationBuilder.DropTable(
                name: "SubGrupoCuentaTranslation");

            migrationBuilder.DropTable(
                name: "SubLineaTranslation");

            migrationBuilder.DropTable(
                name: "SystemConfigurationTranslation");

            migrationBuilder.DropTable(
                name: "TransaccionTranslation");

            migrationBuilder.DropTable(
                name: "UnidadMedidaTranslation");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "CajaRegistradora");

            migrationBuilder.DropTable(
                name: "ComprobanteDetalle");

            migrationBuilder.DropTable(
                name: "ComprobanteTemporal");

            migrationBuilder.DropTable(
                name: "Moneda");

            migrationBuilder.DropTable(
                name: "OperacionesDetalle");

            migrationBuilder.DropTable(
                name: "IdTurno");

            migrationBuilder.DropTable(
                name: "Comprobante");

            migrationBuilder.DropTable(
                name: "Localidad");

            migrationBuilder.DropTable(
                name: "OperacionesEncabezado");

            migrationBuilder.DropTable(
                name: "Producto");

            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropTable(
                name: "ConceptoAjuste");

            migrationBuilder.DropTable(
                name: "Transaccion");

            migrationBuilder.DropTable(
                name: "Establecimiento");

            migrationBuilder.DropTable(
                name: "Proveedor");

            migrationBuilder.DropTable(
                name: "SubLinea");

            migrationBuilder.DropTable(
                name: "UnidadMedida");

            migrationBuilder.DropTable(
                name: "SystemConfiguration");

            migrationBuilder.DropTable(
                name: "Linea");

            migrationBuilder.DropTable(
                name: "Cuenta");

            migrationBuilder.DropTable(
                name: "SubGrupoCuenta");

            migrationBuilder.DropTable(
                name: "GrupoCuenta");
        }
    }
}
