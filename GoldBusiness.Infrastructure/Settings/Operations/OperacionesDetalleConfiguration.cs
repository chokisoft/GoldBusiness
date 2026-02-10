using GoldBusiness.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.Operations
{
    public class OperacionesDetalleConfiguration : IEntityTypeConfiguration<OperacionesDetalle>
    {
        public void Configure(EntityTypeBuilder<OperacionesDetalle> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => e.ProductoId);
            builder.HasIndex(e => e.LocalidadId);
            builder.HasIndex(e => e.OperacionesEncabezadoId);
            builder.Property(e => e.Cantidad).HasColumnType("numeric(18, 3)").HasDefaultValueSql("((0.00))");
            builder.Property(e => e.Costo).HasColumnType("numeric(18, 2)").HasDefaultValueSql("((0.00))");
            builder.Property(e => e.ImporteCosto).HasColumnType("numeric(18, 2)").HasDefaultValueSql("((0.00))");
            builder.Property(e => e.Venta).HasColumnType("numeric(18, 2)").HasDefaultValueSql("((0.00))");
            builder.Property(e => e.ImporteVenta).HasColumnType("numeric(18, 2)").HasDefaultValueSql("((0.00))");
            builder.Property(e => e.Existencia).HasColumnType("numeric(18, 3)").HasDefaultValueSql("((0.00))");
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.Ignore(e => e.ErroresVenta);
            builder.Ignore(e => e.OperacionesServicio);
            builder.HasOne(d => d.Producto).WithMany(p => p.OperacionesDetalle).HasForeignKey(d => d.ProductoId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_OperacionesDetalle_Producto");
            builder.HasOne(d => d.Localidad).WithMany(p => p.OperacionesDetalle).HasForeignKey(d => d.LocalidadId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_OperacionesDetalle_Localidad");
            builder.HasOne(d => d.OperacionEncabezado).WithMany().HasForeignKey(d => d.OperacionesEncabezadoId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_OperacionesDetalle_OperacionesEncabezado");
        }
    }
}
