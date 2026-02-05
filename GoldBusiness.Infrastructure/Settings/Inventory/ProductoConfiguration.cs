using GoldBusiness.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.Inventory
{
    public class ProductoConfiguration : IEntityTypeConfiguration<Producto>
    {
        public void Configure(EntityTypeBuilder<Producto> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => e.ProveedorId);
            builder.HasIndex(e => e.SubLineaId);
            builder.HasIndex(e => e.UnidadMedidaId);
            builder.HasIndex(e => new { e.EstablecimientoId, e.Codigo, e.Cancelado }).HasDatabaseName("IX_Producto").IsUnique();
            builder.Property(e => e.Codigo).IsRequired().HasMaxLength(13);
            builder.Property(e => e.CodigoReferencia).HasMaxLength(50);
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
            builder.Property(e => e.Caracteristicas).HasMaxLength(1024);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.Property(e => e.Imagen).HasColumnType("image");
            builder.Property(e => e.Iva).HasColumnType("decimal(5, 2)").HasDefaultValueSql("((0.00))");
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.PrecioCosto).HasColumnType("decimal(18, 2)").HasDefaultValueSql("((0.00))");
            builder.Property(e => e.PrecioVenta).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.StockMinimo).HasColumnType("numeric(18, 2)").HasDefaultValueSql("((0.00))");
            builder.Ignore(e => e.Translations);
            builder.Ignore(e => e.ErroresVenta);
            builder.Ignore(e => e.OperacionesDetalle);
            builder.Ignore(e => e.OperacionesServicio);
            builder.Ignore(e => e.Saldos);
            builder.Ignore(e => e.SaldosAnteriores);
            builder.HasOne(d => d.EstablecimientoNavigation).WithMany(p => p.Productos).HasForeignKey(d => d.EstablecimientoId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Producto_Establecimiento");
            builder.HasOne(d => d.ProveedorNavigation).WithMany(p => p.Productos).HasForeignKey(d => d.ProveedorId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Producto_Proveedor");
            builder.HasOne(d => d.SubLineaNavigation).WithMany(p => p.Productos).HasForeignKey(d => d.SubLineaId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Producto_SubLinea");
            builder.HasOne(d => d.UnidadMedidaNavigation).WithMany(p => p.Productos).HasForeignKey(d => d.UnidadMedidaId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Producto_UnidadMedida");
        }
    }
}
