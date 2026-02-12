using GoldBusiness.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.EstablishmentLocations
{
    public class LocalidadConfiguration : IEntityTypeConfiguration<Localidad>
    {
        public void Configure(EntityTypeBuilder<Localidad> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => new { e.Codigo, e.EstablecimientoId, e.Cancelado }).HasDatabaseName("IX_Localidad").IsUnique();
            builder.Property(e => e.Activo).HasDefaultValue(true).HasSentinel(false);
            builder.Property(e => e.Codigo).IsRequired().HasMaxLength(9);
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Ignore(e => e.Translations);
            builder.Ignore(e => e.ErroresVenta);
            builder.Ignore(e => e.FichaProductos);
            builder.Ignore(e => e.OperacionesDetalle);
            builder.Ignore(e => e.Saldos);
            builder.Ignore(e => e.SaldosAnteriores);
            builder.HasOne(d => d.Establecimiento).WithMany(p => p.Localidades).HasForeignKey(d => d.EstablecimientoId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Localidad_Establecimiento");
            builder.HasOne(d => d.CuentaInventario).WithMany(p => p.LocalidadCuentaInventario).HasForeignKey(d => d.CuentaInventarioId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Localidad_CuentaInventario");
            builder.HasOne(d => d.CuentaCosto).WithMany(p => p.LocalidadCuentaCosto).HasForeignKey(d => d.CuentaCostoId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Localidad_CuentaCosto");
            builder.HasOne(d => d.CuentaVenta).WithMany(p => p.LocalidadCuentaVenta).HasForeignKey(d => d.CuentaVentaId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Localidad_CuentaVenta");
            builder.HasOne(d => d.CuentaDevolucion).WithMany(p => p.LocalidadCuentaDevolucion).HasForeignKey(d => d.CuentaDevolucionId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Localidad_CuentaDevolucion");
        }
    }
}
