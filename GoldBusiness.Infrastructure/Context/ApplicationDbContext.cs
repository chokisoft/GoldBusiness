using GoldBusiness.Domain.Entities;
using GoldBusiness.Domain.Translation;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GoldBusiness.Infrastructure.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // ═══════════════════════════════════════════════════════════════
        // 📚 DbSets - COMMONS
        // ═══════════════════════════════════════════════════════════════

        public DbSet<Pais> Pais { get; set; }
        public DbSet<PaisTranslation> PaisTranslation { get; set; }
        public DbSet<Provincia> Provincia { get; set; } = null!;
        public DbSet<ProvinciaTranslation> ProvinciaTranslation { get; set; } = null!;
        public DbSet<Municipio> Municipio { get; set; } = null!;
        public DbSet<MunicipioTranslation> MunicipioTranslation { get; set; } = null!;
        public DbSet<CodigoPostal> CodigoPostal { get; set; }

        // ═══════════════════════════════════════════════════════════════
        // 📚 DbSets - TODAS LAS ENTIDADES
        // ═══════════════════════════════════════════════════════════════

        // Configuración General
        public DbSet<SystemConfiguration> SystemConfiguration { get; set; } = null!;
        public DbSet<SystemConfigurationTranslation> SystemConfigurationTranslation { get; set; } = null!;

        // Para Refresh Tokens
        public DbSet<RefreshToken> RefreshToken { get; set; }

        // Plan de Cuentas
        public DbSet<GrupoCuenta> GrupoCuenta { get; set; } = null!;
        public DbSet<GrupoCuentaTranslation> GrupoCuentaTranslation { get; set; } = null!;
        public DbSet<SubGrupoCuenta> SubGrupoCuenta { get; set; } = null!;
        public DbSet<SubGrupoCuentaTranslation> SubGrupoCuentaTranslation { get; set; } = null!;
        public DbSet<Cuenta> Cuenta { get; set; } = null!;
        public DbSet<CuentaTranslation> CuentaTranslation { get; set; } = null!;

        // Estructura Organizacional
        public DbSet<Establecimiento> Establecimiento { get; set; } = null!;
        public DbSet<EstablecimientoTranslation> EstablecimientoTranslation { get; set; } = null!;
        public DbSet<Localidad> Localidad { get; set; } = null!;
        public DbSet<LocalidadTranslation> LocalidadTranslation { get; set; } = null!;
        public DbSet<Moneda> Moneda { get; set; } = null!;
        public DbSet<MonedaTranslation> MonedaTranslation { get; set; } = null!;

        // Inventario - Clasificación
        public DbSet<Linea> Linea { get; set; } = null!;
        public DbSet<LineaTranslation> LineaTranslation { get; set; } = null!;
        public DbSet<SubLinea> SubLinea { get; set; } = null!;
        public DbSet<SubLineaTranslation> SubLineaTranslation { get; set; } = null!;
        public DbSet<UnidadMedida> UnidadMedida { get; set; } = null!;
        public DbSet<UnidadMedidaTranslation> UnidadMedidaTranslation { get; set; } = null!;

        // Inventario - Productos
        public DbSet<Producto> Producto { get; set; } = null!;
        public DbSet<ProductoTranslation> ProductoTranslation { get; set; } = null!;
        public DbSet<FichaProducto> FichaProducto { get; set; } = null!;
        public DbSet<Saldo> Saldo { get; set; } = null!;
        public DbSet<SaldoAnterior> SaldoAnterior { get; set; } = null!;

        // Terceros
        public DbSet<Proveedor> Proveedor { get; set; } = null!;
        public DbSet<ProveedorTranslation> ProveedorTranslation { get; set; } = null!;
        public DbSet<Cliente> Cliente { get; set; } = null!;
        public DbSet<ClienteTranslation> ClienteTranslation { get; set; } = null!;

        // Operaciones
        public DbSet<Transaccion> Transaccion { get; set; } = null!;
        public DbSet<TransaccionTranslation> TransaccionTranslation { get; set; } = null!;
        public DbSet<ConceptoAjuste> ConceptoAjuste { get; set; } = null!;
        public DbSet<ConceptoAjusteTranslation> ConceptoAjusteTranslation { get; set; } = null!;
        public DbSet<OperacionesEncabezado> OperacionesEncabezado { get; set; } = null!;
        public DbSet<OperacionesEncabezadoTranslation> OperacionesEncabezadoTranslation { get; set; } = null!;
        public DbSet<OperacionesDetalle> OperacionesDetalle { get; set; } = null!;
        public DbSet<OperacionesServicio> OperacionesServicio { get; set; } = null!;
        public DbSet<ErroresVenta> ErroresVenta { get; set; } = null!;

        // Contabilidad
        public DbSet<Comprobante> Comprobante { get; set; } = null!;
        public DbSet<ComprobanteTranslation> ComprobanteTranslation { get; set; } = null!;
        public DbSet<ComprobanteDetalle> ComprobanteDetalle { get; set; } = null!;
        public DbSet<ComprobanteDetalleTranslation> ComprobanteDetalleTranslation { get; set; } = null!;
        public DbSet<ComprobanteTemporal> ComprobanteTemporal { get; set; } = null!;
        public DbSet<ComprobanteTemporalTranslation> ComprobanteTemporalTranslation { get; set; } = null!;
        public DbSet<EstadoCuenta> EstadoCuenta { get; set; } = null!;

        // Finanzas
        public DbSet<CuentaCobrarPagar> CuentaCobrarPagar { get; set; } = null!;

        // Caja
        public DbSet<IdTurno> IdTurno { get; set; } = null!;
        public DbSet<CajaRegistradora> CajaRegistradora { get; set; } = null!;
        public DbSet<CajaRegistradoraDetalle> CajaRegistradoraDetalle { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ═══════════════════════════════════════════════════════════════
            // 🔧 APLICAR TODAS LAS CONFIGURACIONES DE UNA SOLA VEZ
            // ═══════════════════════════════════════════════════════════════

            // Aplica TODAS las configuraciones IEntityTypeConfiguration del ensamblado Infrastructure
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            // ═══════════════════════════════════════════════════════════════
            // 🔧 RESTRICCIÓN GLOBAL: Evitar cascadas en Cuenta
            // ═══════════════════════════════════════════════════════════════
            ApplyGlobalRestrictToCuenta(modelBuilder);
        }

        private void ApplyGlobalRestrictToCuenta(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())
                .Where(fk => fk.PrincipalEntityType.ClrType == typeof(Cuenta)))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}