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
        // 📚 DbSets - TODAS LAS ENTIDADES
        // ═══════════════════════════════════════════════════════════════

        // Plan de Cuentas
        public DbSet<Configuracion> Configuracion { get; set; } = null!;
        public DbSet<ConfiguracionTranslation> ConfiguracionTranslation { get; set; } = null!;
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
            // 🔧 CONFIGURACIÓN - Configuracion
            // ═══════════════════════════════════════════════════════════════

            modelBuilder.Entity<Configuracion>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CodigoSistema).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Licencia).IsRequired().HasMaxLength(100);
                entity.Property(e => e.NombreNegocio).IsRequired().HasMaxLength(256);
                entity.Property(e => e.Direccion).HasMaxLength(512);
                entity.Property(e => e.Municipio).HasMaxLength(128);
                entity.Property(e => e.Provincia).HasMaxLength(128);
                entity.Property(e => e.CodPostal).HasMaxLength(20);
                entity.Property(e => e.Imagen).HasColumnType("image");
                entity.Property(e => e.Web).HasMaxLength(256);
                entity.Property(e => e.Email).HasMaxLength(256);
                entity.Property(e => e.Telefono).HasMaxLength(20);
                entity.Property(e => e.Caducidad).HasColumnType("datetime");
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.Ignore(e => e.Translations);
                entity.Ignore(e => e.Establecimientos);
                entity.HasOne(d => d.CuentaPagarNavigation).WithMany(p => p.ConfiguracionCuentaPagarNavigation).HasForeignKey(d => d.CuentaPagarId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Configuracion_CuentaPagar");
                entity.HasOne(d => d.CuentaCobrarNavigation).WithMany(p => p.ConfiguracionCuentaCobrarNavigation).HasForeignKey(d => d.CuentaCobrarId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Configuracion_CuentaCobrar");
            });

            modelBuilder.Entity<ConfiguracionTranslation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.ConfiguracionId, e.Language }).IsUnique();
                entity.Property(e => e.Language).IsRequired().HasMaxLength(5);
                entity.Property(e => e.NombreNegocio).IsRequired().HasMaxLength(256);
                entity.Property(e => e.Direccion).HasMaxLength(512);
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.Ignore(e => e.Configuracion);
                entity.HasOne<Configuracion>().WithMany().HasForeignKey(e => e.ConfiguracionId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_ConfiguracionTranslation_Configuracion");
            });

            // ═══════════════════════════════════════════════════════════════
            // 🔧 PLAN DE CUENTAS
            // ═══════════════════════════════════════════════════════════════

            modelBuilder.Entity<GrupoCuenta>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.Descripcion, e.Cancelado }).HasDatabaseName("IX_GrupoCuenta").IsUnique();
                entity.Property(e => e.Codigo).IsRequired().HasMaxLength(2);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.Ignore(e => e.Translations);
                entity.Ignore(e => e.SubGrupoCuenta);
            });

            modelBuilder.Entity<GrupoCuentaTranslation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.GrupoCuentaId, e.Language }).IsUnique();
                entity.Property(e => e.Language).IsRequired().HasMaxLength(5);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.HasOne(e => e.GrupoCuenta).WithMany(g => g.Translations).HasForeignKey(e => e.GrupoCuentaId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<SubGrupoCuenta>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.GrupoCuentaId);
                entity.HasIndex(e => new { e.Codigo, e.GrupoCuentaId, e.Cancelado }).HasDatabaseName("IX_SubGrupoCuenta").IsUnique();
                entity.Property(e => e.Codigo).IsRequired().HasMaxLength(5);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.Ignore(e => e.Translations);
                entity.Ignore(e => e.Cuenta);
                entity.HasOne(d => d.GrupoCuenta).WithMany(p => p.SubGrupoCuenta).HasForeignKey(d => d.GrupoCuentaId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_SubGrupoCuenta_GrupoCuenta");
            });

            modelBuilder.Entity<SubGrupoCuentaTranslation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.SubGrupoCuentaId, e.Language }).IsUnique();
                entity.Property(e => e.Language).IsRequired().HasMaxLength(5);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.HasOne(e => e.SubGrupoCuenta).WithMany(s => s.Translations).HasForeignKey(e => e.SubGrupoCuentaId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Cuenta>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.SubGrupoCuentaId).HasDatabaseName("IX_Cuenta_SubGrupo");
                entity.HasIndex(e => new { e.Codigo, e.SubGrupoCuentaId, e.Cancelado }).HasDatabaseName("IX_Cuenta").IsUnique();
                entity.Property(e => e.Codigo).IsRequired().HasMaxLength(8);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.Ignore(e => e.Translations);
                entity.Ignore(e => e.LocalidadCuentaInventarioNavigation);
                entity.Ignore(e => e.LocalidadCuentaCostoNavigation);
                entity.Ignore(e => e.LocalidadCuentaVentaNavigation);
                entity.Ignore(e => e.LocalidadCuentaDevolucionNavigation);
                entity.Ignore(e => e.ConfiguracionCuentaPagarNavigation);
                entity.Ignore(e => e.ConfiguracionCuentaCobrarNavigation);
                entity.HasOne(d => d.SubGrupoCuenta).WithMany(p => p.Cuenta).HasForeignKey(d => d.SubGrupoCuentaId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Cuenta_SubGrupoCuenta");
            });

            modelBuilder.Entity<CuentaTranslation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.CuentaId, e.Language }).IsUnique();
                entity.Property(e => e.Language).IsRequired().HasMaxLength(5);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.HasOne(e => e.Cuenta).WithMany(c => c.Translations).HasForeignKey(e => e.CuentaId).OnDelete(DeleteBehavior.Cascade);
            });

            // ═══════════════════════════════════════════════════════════════
            // 🔧 ESTABLECIMIENTO Y LOCALIDADES
            // ═══════════════════════════════════════════════════════════════

            modelBuilder.Entity<Establecimiento>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.NegocioId, e.Codigo, e.Cancelado }).HasDatabaseName("IX_Establecimiento").IsUnique();
                entity.Property(e => e.Activo).IsRequired().HasDefaultValueSql("((1))");
                entity.Property(e => e.Codigo).IsRequired().HasMaxLength(6);
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Ignore(e => e.Translations);
                entity.Ignore(e => e.Comprobantes);
                entity.Ignore(e => e.ComprobantesTemporales);
                entity.Ignore(e => e.CuentasCobrarPagar);
                entity.Ignore(e => e.EstadosCuenta);
                entity.Ignore(e => e.Localidades);
                entity.Ignore(e => e.Monedas);
                entity.Ignore(e => e.OperacionesEncabezado);
                entity.Ignore(e => e.Productos);
                entity.HasOne(d => d.NegocioNavigation).WithMany(p => p.Establecimientos).HasForeignKey(d => d.NegocioId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Establecimiento_Configuracion");
            });

            modelBuilder.Entity<EstablecimientoTranslation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.EstablecimientoId, e.Language }).IsUnique();
                entity.Property(e => e.Language).IsRequired().HasMaxLength(5);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.Ignore(e => e.Establecimiento);
                entity.HasOne<Establecimiento>().WithMany().HasForeignKey(e => e.EstablecimientoId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_EstablecimientoTranslation_Establecimiento");
            });

            modelBuilder.Entity<Localidad>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.EstablecimientoId, e.Codigo, e.Cancelado }).HasDatabaseName("IX_Localidad").IsUnique();
                entity.Property(e => e.Activo).IsRequired().HasDefaultValueSql("((1))");
                entity.Property(e => e.Codigo).IsRequired().HasMaxLength(9);
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Ignore(e => e.Translations);
                entity.Ignore(e => e.ErroresVenta);
                entity.Ignore(e => e.FichaProductos);
                entity.Ignore(e => e.OperacionesDetalle);
                entity.Ignore(e => e.Saldos);
                entity.Ignore(e => e.SaldosAnteriores);
                entity.HasOne(d => d.EstablecimientoNavigation).WithMany(p => p.Localidades).HasForeignKey(d => d.EstablecimientoId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Localidad_Establecimiento");
                entity.HasOne(d => d.CuentaInventarioNavigation).WithMany(p => p.LocalidadCuentaInventarioNavigation).HasForeignKey(d => d.CuentaInventarioId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Localidad_CuentaInventario");
                entity.HasOne(d => d.CuentaCostoNavigation).WithMany(p => p.LocalidadCuentaCostoNavigation).HasForeignKey(d => d.CuentaCostoId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Localidad_CuentaCosto");
                entity.HasOne(d => d.CuentaVentaNavigation).WithMany(p => p.LocalidadCuentaVentaNavigation).HasForeignKey(d => d.CuentaVentaId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Localidad_CuentaVenta");
                entity.HasOne(d => d.CuentaDevolucionNavigation).WithMany(p => p.LocalidadCuentaDevolucionNavigation).HasForeignKey(d => d.CuentaDevolucionId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Localidad_CuentaDevolucion");
            });

            modelBuilder.Entity<LocalidadTranslation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.LocalidadId, e.Language }).IsUnique();
                entity.Property(e => e.Language).IsRequired().HasMaxLength(5);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.Ignore(e => e.Localidad);
                entity.HasOne<Localidad>().WithMany().HasForeignKey(e => e.LocalidadId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_LocalidadTranslation_Localidad");
            });

            modelBuilder.Entity<Moneda>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.EstablecimientoId, e.Codigo, e.Cancelado }).IsUnique();
                entity.Property(e => e.Codigo).IsRequired().HasMaxLength(3);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.Ignore(e => e.Translations);
                entity.HasOne(d => d.EstablecimientoNavigation).WithMany(p => p.Monedas).HasForeignKey(d => d.EstablecimientoId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Moneda_Establecimiento");
            });

            modelBuilder.Entity<MonedaTranslation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.MonedaId, e.Language }).IsUnique();
                entity.Property(e => e.Language).IsRequired().HasMaxLength(5);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.Ignore(e => e.Moneda);
                entity.HasOne<Moneda>().WithMany().HasForeignKey(e => e.MonedaId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_MonedaTranslation_Moneda");
            });

            // ═══════════════════════════════════════════════════════════════
            // 🔧 INVENTARIO - CLASIFICACIÓN
            // ═══════════════════════════════════════════════════════════════

            modelBuilder.Entity<Linea>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.Codigo, e.Cancelado }).HasDatabaseName("IX_Linea").IsUnique();
                entity.Property(e => e.Codigo).IsRequired().HasMaxLength(2);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.Ignore(e => e.Translations);
                entity.Ignore(e => e.SubLineas);
            });

            modelBuilder.Entity<LineaTranslation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.LineaId, e.Language }).IsUnique();
                entity.Property(e => e.Language).IsRequired().HasMaxLength(5);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.HasOne(e => e.Linea).WithMany(g => g.Translations).HasForeignKey(e => e.LineaId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<SubLinea>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.LineaId);
                entity.HasIndex(e => new { e.Codigo, e.LineaId, e.Cancelado }).HasDatabaseName("IX_SubLinea").IsUnique();
                entity.Property(e => e.Codigo).IsRequired().HasMaxLength(5);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.Ignore(e => e.Translations);
                entity.Ignore(e => e.Productos);
                entity.HasOne(d => d.LineaNavigation).WithMany(p => p.SubLineas).HasForeignKey(d => d.LineaId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_SubLinea_Linea");
            });

            modelBuilder.Entity<SubLineaTranslation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.SubLineaId, e.Language }).IsUnique();
                entity.Property(e => e.Language).IsRequired().HasMaxLength(5);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.HasOne(e => e.SubLinea).WithMany(s => s.Translations).HasForeignKey(e => e.SubLineaId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<UnidadMedida>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.Codigo, e.Cancelado }).HasDatabaseName("IX_UnidadMedida").IsUnique();
                entity.Property(e => e.Codigo).IsRequired().HasMaxLength(3);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.Ignore(e => e.Translations);
                entity.Ignore(e => e.Productos);
            });

            modelBuilder.Entity<UnidadMedidaTranslation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.UnidadMedidaId, e.Language }).IsUnique();
                entity.Property(e => e.Language).IsRequired().HasMaxLength(5);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.Ignore(e => e.UnidadMedida);
                entity.HasOne<UnidadMedida>().WithMany().HasForeignKey(e => e.UnidadMedidaId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_UnidadMedidaTranslation_UnidadMedida");
            });

            // ═══════════════════════════════════════════════════════════════
            // 🔧 TERCEROS
            // ═══════════════════════════════════════════════════════════════

            modelBuilder.Entity<Proveedor>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.Codigo, e.Cancelado }).HasDatabaseName("IX_Proveedor").IsUnique();
                entity.Property(e => e.Codigo).IsRequired().HasMaxLength(5);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
                entity.Property(e => e.Nif).HasMaxLength(11);
                entity.Property(e => e.Iban).HasMaxLength(27);
                entity.Property(e => e.BicoSwift).HasMaxLength(11);
                entity.Property(e => e.Iva).HasColumnType("decimal(5, 2)");
                entity.Property(e => e.Direccion).HasMaxLength(256);
                entity.Property(e => e.Municipio).HasMaxLength(50);
                entity.Property(e => e.Provincia).HasMaxLength(50);
                entity.Property(e => e.CodPostal).HasMaxLength(5);
                entity.Property(e => e.Web).HasMaxLength(256);
                entity.Property(e => e.Email1).HasMaxLength(256);
                entity.Property(e => e.Email2).HasMaxLength(256);
                entity.Property(e => e.Telefono1).HasMaxLength(50);
                entity.Property(e => e.Telefono2).HasMaxLength(50);
                entity.Property(e => e.Fax1).HasMaxLength(50);
                entity.Property(e => e.Fax2).HasMaxLength(50);
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.Ignore(e => e.Translations);
                entity.Ignore(e => e.CuentasCobrarPagar);
                entity.Ignore(e => e.OperacionesEncabezado);
                entity.Ignore(e => e.Productos);
            });

            modelBuilder.Entity<ProveedorTranslation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.ProveedorId, e.Language }).IsUnique();
                entity.Property(e => e.Language).IsRequired().HasMaxLength(5);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.Ignore(e => e.Proveedor);
                entity.HasOne<Proveedor>().WithMany().HasForeignKey(e => e.ProveedorId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_ProveedorTranslation_Proveedor");
            });

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.Codigo, e.Cancelado }).HasDatabaseName("IX_Cliente").IsUnique();
                entity.Property(e => e.Codigo).IsRequired().HasMaxLength(8);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
                entity.Property(e => e.Nif).HasMaxLength(11);
                entity.Property(e => e.Iban).HasMaxLength(27);
                entity.Property(e => e.BicoSwift).HasMaxLength(11);
                entity.Property(e => e.Iva).HasColumnType("decimal(5, 2)");
                entity.Property(e => e.Direccion).HasMaxLength(256);
                entity.Property(e => e.Municipio).HasMaxLength(50);
                entity.Property(e => e.Provincia).HasMaxLength(50);
                entity.Property(e => e.CodPostal).HasMaxLength(5);
                entity.Property(e => e.Web).HasMaxLength(256);
                entity.Property(e => e.Email1).HasMaxLength(256);
                entity.Property(e => e.Email2).HasMaxLength(256);
                entity.Property(e => e.Telefono1).HasMaxLength(50);
                entity.Property(e => e.Telefono2).HasMaxLength(50);
                entity.Property(e => e.Fax1).HasMaxLength(50);
                entity.Property(e => e.Fax2).HasMaxLength(50);
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.Ignore(e => e.Translations);
                entity.Ignore(e => e.CuentasCobrarPagar);
                entity.Ignore(e => e.OperacionesEncabezado);
            });

            modelBuilder.Entity<ClienteTranslation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.ClienteId, e.Language }).IsUnique();
                entity.Property(e => e.Language).IsRequired().HasMaxLength(5);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.Ignore(e => e.Cliente);
                entity.HasOne<Cliente>().WithMany().HasForeignKey(e => e.ClienteId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_ClienteTranslation_Cliente");
            });

            // ═══════════════════════════════════════════════════════════════
            // 🔧 INVENTARIO - PRODUCTOS
            // ═══════════════════════════════════════════════════════════════

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.ProveedorId);
                entity.HasIndex(e => e.SubLineaId);
                entity.HasIndex(e => e.UnidadMedidaId);
                entity.HasIndex(e => new { e.EstablecimientoId, e.Codigo, e.Cancelado }).HasDatabaseName("IX_Producto").IsUnique();
                entity.Property(e => e.Codigo).IsRequired().HasMaxLength(13);
                entity.Property(e => e.CodigoReferencia).HasMaxLength(50);
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
                entity.Property(e => e.Caracteristicas).HasMaxLength(1024);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.Property(e => e.Imagen).HasColumnType("image");
                entity.Property(e => e.Iva).HasColumnType("decimal(5, 2)").HasDefaultValueSql("((0.00))");
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.PrecioCosto).HasColumnType("decimal(18, 2)").HasDefaultValueSql("((0.00))");
                entity.Property(e => e.PrecioVenta).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.StockMinimo).HasColumnType("numeric(18, 2)").HasDefaultValueSql("((0.00))");
                entity.Ignore(e => e.Translations);
                entity.Ignore(e => e.ErroresVenta);
                entity.Ignore(e => e.FichaProductoCodigoNavigation);
                entity.Ignore(e => e.FichaProductoProductoNavigation);
                entity.Ignore(e => e.OperacionesDetalle);
                entity.Ignore(e => e.OperacionesServicio);
                entity.Ignore(e => e.Saldos);
                entity.Ignore(e => e.SaldosAnteriores);
                // ✅ FK COMPLETAS (DESCOMENTADAS)
                entity.HasOne(d => d.EstablecimientoNavigation).WithMany(p => p.Productos).HasForeignKey(d => d.EstablecimientoId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Producto_Establecimiento");
                entity.HasOne(d => d.ProveedorNavigation).WithMany(p => p.Productos).HasForeignKey(d => d.ProveedorId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Producto_Proveedor");
                entity.HasOne(d => d.SubLineaNavigation).WithMany(p => p.Productos).HasForeignKey(d => d.SubLineaId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Producto_SubLinea");
                entity.HasOne(d => d.UnidadMedidaNavigation).WithMany(p => p.Productos).HasForeignKey(d => d.UnidadMedidaId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Producto_UnidadMedida");
            });

            modelBuilder.Entity<ProductoTranslation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.ProductoId, e.Language }).IsUnique();
                entity.Property(e => e.Language).IsRequired().HasMaxLength(5);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
                entity.Property(e => e.Caracteristicas).HasMaxLength(1024);
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.Ignore(e => e.Producto);
                entity.HasOne<Producto>().WithMany().HasForeignKey(e => e.ProductoId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_ProductoTranslation_Producto");
            });

            modelBuilder.Entity<FichaProducto>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.ProductoId);
                entity.HasIndex(e => e.ComponenteId);
                entity.HasIndex(e => e.LocalidadId);
                entity.Property(e => e.Cantidad).HasColumnType("numeric(18, 3)");
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.HasOne(d => d.ProductoNavigation).WithMany(p => p.FichaProductoProductoNavigation).HasForeignKey(d => d.ProductoId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_FichaProducto_Producto");
                entity.HasOne(d => d.ComponenteNavigation).WithMany(p => p.FichaProductoCodigoNavigation).HasForeignKey(d => d.ComponenteId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_FichaProducto_Componente");
                entity.HasOne(d => d.LocalidadNavigation).WithMany(p => p.FichaProductos).HasForeignKey(d => d.LocalidadId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_FichaProducto_Localidad");
            });

            modelBuilder.Entity<Saldo>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.LocalidadId);
                entity.HasIndex(e => e.ProductoId);
                entity.HasIndex(e => new { e.LocalidadId, e.ProductoId }).IsUnique().HasDatabaseName("IX_Saldo_Localidad_Producto");
                entity.Property(e => e.Existencia).HasColumnType("numeric(18, 2)");
                entity.Property(e => e.Fecha).HasColumnType("datetime");
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.HasOne(d => d.LocalidadNavigation).WithMany(p => p.Saldos).HasForeignKey(d => d.LocalidadId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Saldo_Localidad");
                entity.HasOne(d => d.ProductoNavigation).WithMany(p => p.Saldos).HasForeignKey(d => d.ProductoId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Saldo_Producto");
            });

            modelBuilder.Entity<SaldoAnterior>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.LocalidadId);
                entity.HasIndex(e => e.ProductoId);
                entity.HasIndex(e => new { e.LocalidadId, e.ProductoId, e.Fecha }).HasDatabaseName("IX_SaldoAnterior_Localidad_Producto_Fecha");
                entity.Property(e => e.Existencia).HasColumnType("numeric(18, 3)");
                entity.Property(e => e.ImporteCosto).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.PrecioCosto).HasColumnType("decimal(18, 2)").HasDefaultValueSql("((0.00))");
                entity.Property(e => e.Fecha).HasColumnType("datetime");
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.HasOne(d => d.LocalidadNavigation).WithMany(p => p.SaldosAnteriores).HasForeignKey(d => d.LocalidadId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_SaldoAnterior_Localidad");
                entity.HasOne(d => d.ProductoNavigation).WithMany(p => p.SaldosAnteriores).HasForeignKey(d => d.ProductoId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_SaldoAnterior_Producto");
            });

            // ═══════════════════════════════════════════════════════════════
            // 🔧 OPERACIONES
            // ═══════════════════════════════════════════════════════════════

            modelBuilder.Entity<Transaccion>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Codigo).IsRequired().HasMaxLength(10);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.Ignore(e => e.Translations);
                entity.Ignore(e => e.CuentasCobrarPagar);
                entity.Ignore(e => e.OperacionesEncabezado);
            });

            modelBuilder.Entity<TransaccionTranslation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.TransaccionId, e.Language }).IsUnique();
                entity.Property(e => e.Language).IsRequired().HasMaxLength(5);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.Ignore(e => e.Transaccion);
                entity.HasOne<Transaccion>().WithMany().HasForeignKey(e => e.TransaccionId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_TransaccionTranslation_Transaccion");
            });

            modelBuilder.Entity<ConceptoAjuste>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Codigo).IsRequired().HasMaxLength(10);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.Ignore(e => e.Translations);
                entity.Ignore(e => e.OperacionesEncabezado);
                entity.HasOne(d => d.CuentaNavigation).WithMany().HasForeignKey(d => d.CuentaId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_ConceptoAjuste_Cuenta");
            });

            modelBuilder.Entity<ConceptoAjusteTranslation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.ConceptoAjusteId, e.Language }).IsUnique();
                entity.Property(e => e.Language).IsRequired().HasMaxLength(5);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.Ignore(e => e.ConceptoAjuste);
                entity.HasOne<ConceptoAjuste>().WithMany().HasForeignKey(e => e.ConceptoAjusteId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_ConceptoAjusteTranslation_ConceptoAjuste");
            });

            modelBuilder.Entity<OperacionesEncabezado>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.EstablecimientoId);
                entity.HasIndex(e => e.TransaccionId);
                entity.HasIndex(e => e.ProveedorId);
                entity.HasIndex(e => e.ClienteId);
                entity.HasIndex(e => e.ConceptoAjusteId);
                entity.Property(e => e.NoDocumento).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Fecha).HasColumnType("datetime");
                entity.Property(e => e.NoPrimario).HasMaxLength(50);
                entity.Property(e => e.Concepto).HasMaxLength(512);
                entity.Property(e => e.Observaciones).HasMaxLength(1024);
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.Ignore(e => e.Translations);
                entity.Ignore(e => e.OperacionesDetalle);
                entity.HasOne(d => d.EstablecimientoNavigation).WithMany(p => p.OperacionesEncabezado).HasForeignKey(d => d.EstablecimientoId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_OperacionesEncabezado_Establecimiento");
                entity.HasOne(d => d.TransaccionNavigation).WithMany(p => p.OperacionesEncabezado).HasForeignKey(d => d.TransaccionId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_OperacionesEncabezado_Transaccion");
                entity.HasOne(d => d.ProveedorNavigation).WithMany(p => p.OperacionesEncabezado).HasForeignKey(d => d.ProveedorId).OnDelete(DeleteBehavior.SetNull).HasConstraintName("FK_OperacionesEncabezado_Proveedor");
                entity.HasOne(d => d.ClienteNavigation).WithMany(p => p.OperacionesEncabezado).HasForeignKey(d => d.ClienteId).OnDelete(DeleteBehavior.SetNull).HasConstraintName("FK_OperacionesEncabezado_Cliente");
                entity.HasOne(d => d.ConceptoAjusteNavigation).WithMany(p => p.OperacionesEncabezado).HasForeignKey(d => d.ConceptoAjusteId).OnDelete(DeleteBehavior.SetNull).HasConstraintName("FK_OperacionesEncabezado_ConceptoAjuste");
            });

            modelBuilder.Entity<OperacionesEncabezadoTranslation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.OperacionesEncabezadoId, e.Language }).IsUnique();
                entity.Property(e => e.Language).IsRequired().HasMaxLength(5);
                entity.Property(e => e.Concepto).HasMaxLength(512);
                entity.Property(e => e.Observaciones).HasMaxLength(1024);
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.Ignore(e => e.OperacionesEncabezado);
                entity.HasOne<OperacionesEncabezado>().WithMany().HasForeignKey(e => e.OperacionesEncabezadoId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_OperacionesEncabezadoTranslation_OperacionesEncabezado");
            });

            modelBuilder.Entity<OperacionesDetalle>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.ProductoId);
                entity.HasIndex(e => e.LocalidadId);
                entity.HasIndex(e => e.OperacionesEncabezadoId);
                entity.Property(e => e.Cantidad).HasColumnType("numeric(18, 3)").HasDefaultValueSql("((0.00))");
                entity.Property(e => e.Costo).HasColumnType("numeric(18, 2)").HasDefaultValueSql("((0.00))");
                entity.Property(e => e.ImporteCosto).HasColumnType("numeric(18, 2)").HasDefaultValueSql("((0.00))");
                entity.Property(e => e.Venta).HasColumnType("numeric(18, 2)").HasDefaultValueSql("((0.00))");
                entity.Property(e => e.ImporteVenta).HasColumnType("numeric(18, 2)").HasDefaultValueSql("((0.00))");
                entity.Property(e => e.Existencia).HasColumnType("numeric(18, 3)").HasDefaultValueSql("((0.00))");
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.Ignore(e => e.ErroresVenta);
                entity.Ignore(e => e.OperacionesServicio);
                entity.HasOne(d => d.ProductoNavigation).WithMany(p => p.OperacionesDetalle).HasForeignKey(d => d.ProductoId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_OperacionesDetalle_Producto");
                entity.HasOne(d => d.LocalidadNavigation).WithMany(p => p.OperacionesDetalle).HasForeignKey(d => d.LocalidadId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_OperacionesDetalle_Localidad");
                entity.HasOne(d => d.OperacionEncabezadoNavigation).WithMany(p => p.OperacionesDetalle).HasForeignKey(d => d.OperacionesEncabezadoId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_OperacionesDetalle_OperacionesEncabezado");
            });

            modelBuilder.Entity<OperacionesServicio>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.OperacionesDetalleId);
                entity.HasIndex(e => e.ProductoId);
                entity.Property(e => e.Cantidad).HasColumnType("decimal(18, 3)").HasDefaultValueSql("((0.00))");
                entity.Property(e => e.Costo).HasColumnType("decimal(18, 2)").HasDefaultValueSql("((0.00))");
                entity.Property(e => e.ImporteCosto).HasColumnType("decimal(18, 2)").HasDefaultValueSql("((0.00))");
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.HasOne(d => d.OperacionesDetalleNavigation).WithMany(p => p.OperacionesServicio).HasForeignKey(d => d.OperacionesDetalleId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_OperacionesServicio_OperacionesDetalle");
                entity.HasOne(d => d.ProductoNavigation).WithMany(p => p.OperacionesServicio).HasForeignKey(d => d.ProductoId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_OperacionesServicio_Producto");
            });

            modelBuilder.Entity<ErroresVenta>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.OperacionesDetalleId).HasDatabaseName("IX_ErroresVenta_OperacionesDetalle");
                entity.HasIndex(e => e.LocalidadId).HasDatabaseName("IX_ErroresVenta_Localidad");
                entity.HasIndex(e => e.ProductoId).HasDatabaseName("IX_ErroresVenta_Producto");
                entity.Property(e => e.Cantidad).HasColumnType("numeric(18, 2)");
                entity.Property(e => e.Costo).HasColumnType("numeric(18, 2)");
                entity.Property(e => e.ImporteCosto).HasColumnType("numeric(18, 2)");
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.HasOne(d => d.OperacionesDetalleNavigation).WithMany(p => p.ErroresVenta).HasForeignKey(d => d.OperacionesDetalleId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_ErroresVenta_OperacionesDetalle");
                entity.HasOne(d => d.LocalidadNavigation).WithMany(p => p.ErroresVenta).HasForeignKey(d => d.LocalidadId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_ErroresVenta_Localidad");
                entity.HasOne(d => d.ProductoNavigation).WithMany(p => p.ErroresVenta).HasForeignKey(d => d.ProductoId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_ErroresVenta_Producto");
            });

            // ═══════════════════════════════════════════════════════════════
            // 🔧 CONTABILIDAD
            // ═══════════════════════════════════════════════════════════════

            modelBuilder.Entity<Comprobante>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.EstablecimientoId);
                entity.Property(e => e.NoComprobante).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Fecha).HasColumnType("datetime");
                entity.Property(e => e.Observaciones).HasMaxLength(1024);
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.Ignore(e => e.Translations);
                entity.Ignore(e => e.Detalles);
                entity.HasOne(d => d.EstablecimientoNavigation).WithMany(p => p.Comprobantes).HasForeignKey(d => d.EstablecimientoId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Comprobante_Establecimiento");
            });

            modelBuilder.Entity<ComprobanteTranslation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.ComprobanteId, e.Language }).IsUnique();
                entity.Property(e => e.Language).IsRequired().HasMaxLength(5);
                entity.Property(e => e.Observaciones).HasMaxLength(1024);
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.Ignore(e => e.Comprobante);
                entity.HasOne<Comprobante>().WithMany().HasForeignKey(e => e.ComprobanteId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_ComprobanteTranslation_Comprobante");
            });

            modelBuilder.Entity<ComprobanteDetalle>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.ComprobanteId);
                entity.HasIndex(e => e.CuentaId);
                entity.Property(e => e.Departamento).HasMaxLength(50);
                entity.Property(e => e.Debito).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.Credito).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.Parcial).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.Nota).HasMaxLength(512);
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.Ignore(e => e.Translations);
                entity.HasOne(d => d.ComprobanteNavigation).WithMany(p => p.Detalles).HasForeignKey(d => d.ComprobanteId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_ComprobanteDetalle_Comprobante");
                entity.HasOne(d => d.CuentaNavigation).WithMany().HasForeignKey(d => d.CuentaId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_ComprobanteDetalle_Cuenta");
            });

            modelBuilder.Entity<ComprobanteDetalleTranslation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.ComprobanteDetalleId, e.Language }).IsUnique();
                entity.Property(e => e.Language).IsRequired().HasMaxLength(5);
                entity.Property(e => e.Nota).HasMaxLength(512);
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.Ignore(e => e.ComprobanteDetalle);
                entity.HasOne<ComprobanteDetalle>().WithMany().HasForeignKey(e => e.ComprobanteDetalleId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_ComprobanteDetalleTranslation_ComprobanteDetalle");
            });

            modelBuilder.Entity<ComprobanteTemporal>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.EstablecimientoId);
                entity.Property(e => e.CodigoTransaccion).IsRequired().HasMaxLength(10);
                entity.Property(e => e.Transaccion).IsRequired().HasMaxLength(256);
                entity.Property(e => e.NoDocumento).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Fecha).HasColumnType("datetime");
                entity.Property(e => e.Cuenta).IsRequired().HasMaxLength(8);
                entity.Property(e => e.Departamento).HasMaxLength(50);
                entity.Property(e => e.Descripcion).HasMaxLength(512);
                entity.Property(e => e.Debito).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.Credito).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.Parcial).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Ignore(e => e.Translations);
                entity.HasOne(d => d.EstablecimientoNavigation).WithMany(p => p.ComprobantesTemporales).HasForeignKey(d => d.EstablecimientoId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_ComprobanteTemporal_Establecimiento");
            });

            modelBuilder.Entity<ComprobanteTemporalTranslation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.ComprobanteTemporalId, e.Language }).IsUnique();
                entity.Property(e => e.Language).IsRequired().HasMaxLength(5);
                entity.Property(e => e.Descripcion).HasMaxLength(512);
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.Ignore(e => e.ComprobanteTemporal);
                entity.HasOne<ComprobanteTemporal>().WithMany().HasForeignKey(e => e.ComprobanteTemporalId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_ComprobanteTemporalTranslation_ComprobanteTemporal");
            });

            modelBuilder.Entity<EstadoCuenta>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.EstablecimientoId);
                entity.HasIndex(e => e.CuentaId);
                entity.Property(e => e.Fecha).HasColumnType("datetime");
                entity.Property(e => e.Debito).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.Credito).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.Saldo).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.Referencia).HasMaxLength(256);
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.HasOne(d => d.EstablecimientoNavigation).WithMany(p => p.EstadosCuenta).HasForeignKey(d => d.EstablecimientoId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_EstadoCuenta_Establecimiento");
                entity.HasOne(d => d.CuentaNavigation).WithMany().HasForeignKey(d => d.CuentaId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_EstadoCuenta_Cuenta");
            });

            // ═══════════════════════════════════════════════════════════════
            // 🔧 FINANZAS
            // ═══════════════════════════════════════════════════════════════

            modelBuilder.Entity<CuentaCobrarPagar>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.EstablecimientoId);
                entity.HasIndex(e => e.TransaccionId);
                entity.HasIndex(e => e.ProveedorId);
                entity.HasIndex(e => e.ClienteId);
                entity.Property(e => e.Fecha).HasColumnType("datetime");
                entity.Property(e => e.NoPrimario).HasMaxLength(50);
                entity.Property(e => e.NoDocumento).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Importe).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.PagoEfectivoDepartamento).HasMaxLength(50);
                entity.Property(e => e.PagoEfectivoImporte).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.PagoEfectivoParcialMlc).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.PagoElectronicoDepartamento).HasMaxLength(50);
                entity.Property(e => e.PagoElectronicoImporte).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.PagoElectronicoParcialMlc).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.CobroEfectivoDepartamento).HasMaxLength(50);
                entity.Property(e => e.CobroEfectivoImporte).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.CobroEfectivoParcialMlc).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.CobroElectronicoDepartamento).HasMaxLength(50);
                entity.Property(e => e.CobroElectronicoImporte).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.CobroElectronicoParcialMlc).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.HasOne(d => d.EstablecimientoNavigation).WithMany(p => p.CuentasCobrarPagar).HasForeignKey(d => d.EstablecimientoId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CuentaCobrarPagar_Establecimiento");
                entity.HasOne(d => d.TransaccionNavigation).WithMany(p => p.CuentasCobrarPagar).HasForeignKey(d => d.TransaccionId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CuentaCobrarPagar_Transaccion");
                entity.HasOne(d => d.ProveedorNavigation).WithMany(p => p.CuentasCobrarPagar).HasForeignKey(d => d.ProveedorId).OnDelete(DeleteBehavior.SetNull).HasConstraintName("FK_CuentaCobrarPagar_Proveedor");
                entity.HasOne(d => d.ClienteNavigation).WithMany(p => p.CuentasCobrarPagar).HasForeignKey(d => d.ClienteId).OnDelete(DeleteBehavior.SetNull).HasConstraintName("FK_CuentaCobrarPagar_Cliente");
                entity.HasOne(d => d.CuentaPagoEfectivoNavigation).WithMany().HasForeignKey(d => d.CuentaPagoEfectivoId).OnDelete(DeleteBehavior.SetNull).HasConstraintName("FK_CuentaCobrarPagar_CuentaPagoEfectivo");
                entity.HasOne(d => d.CuentaPagoElectronicoNavigation).WithMany().HasForeignKey(d => d.CuentaPagoElectronicoId).OnDelete(DeleteBehavior.SetNull).HasConstraintName("FK_CuentaCobrarPagar_CuentaPagoElectronico");
                entity.HasOne(d => d.CuentaCobroEfectivoNavigation).WithMany().HasForeignKey(d => d.CuentaCobroEfectivoId).OnDelete(DeleteBehavior.SetNull).HasConstraintName("FK_CuentaCobrarPagar_CuentaCobroEfectivo");
                entity.HasOne(d => d.CuentaCobroElectronicoNavigation).WithMany().HasForeignKey(d => d.CuentaCobroElectronicoId).OnDelete(DeleteBehavior.SetNull).HasConstraintName("FK_CuentaCobrarPagar_CuentaCobroElectronico");
            });

            // ═══════════════════════════════════════════════════════════════
            // 🔧 CAJA
            // ═══════════════════════════════════════════════════════════════

            modelBuilder.Entity<IdTurno>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Fecha).HasColumnType("datetime");
                entity.Property(e => e.Cajero).IsRequired().HasMaxLength(256);
                entity.Property(e => e.Inicio).HasColumnType("datetime");
                entity.Property(e => e.Fondo).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.Extraccion).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.Cierre).HasColumnType("datetime");
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.Ignore(e => e.CajasRegistradoras);
            });

            modelBuilder.Entity<CajaRegistradora>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.IdTurnoId);
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
                entity.Ignore(e => e.Detalles);
                entity.HasOne(d => d.IdTurnoNavigation).WithMany(p => p.CajasRegistradoras).HasForeignKey(d => d.IdTurnoId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CajaRegistradora_IdTurno");
            });

            modelBuilder.Entity<CajaRegistradoraDetalle>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.CajaRegistradoraId);
                entity.HasIndex(e => e.LocalidadId);
                entity.HasIndex(e => e.ProductoId);
                entity.Property(e => e.Cantidad).HasColumnType("decimal(18, 3)");
                entity.Property(e => e.Venta).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.ImporteVenta).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
                entity.HasOne(d => d.CajaRegistradoraNavigation).WithMany(p => p.Detalles).HasForeignKey(d => d.CajaRegistradoraId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CajaRegistradoraDetalle_CajaRegistradora");
                entity.HasOne(d => d.LocalidadNavigation).WithMany().HasForeignKey(d => d.LocalidadId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CajaRegistradoraDetalle_Localidad");
                entity.HasOne(d => d.ProductoNavigation).WithMany().HasForeignKey(d => d.ProductoId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CajaRegistradoraDetalle_Producto");
            });

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