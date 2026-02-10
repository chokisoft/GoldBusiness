using GoldBusiness.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.Operations
{
    public class OperacionesServicioConfiguration : IEntityTypeConfiguration<OperacionesServicio>
    {
        public void Configure(EntityTypeBuilder<OperacionesServicio> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => e.OperacionesDetalleId);
            builder.HasIndex(e => e.ProductoId);
            builder.Property(e => e.Cantidad).HasColumnType("decimal(18, 3)").HasDefaultValueSql("((0.00))");
            builder.Property(e => e.Costo).HasColumnType("decimal(18, 2)").HasDefaultValueSql("((0.00))");
            builder.Property(e => e.ImporteCosto).HasColumnType("decimal(18, 2)").HasDefaultValueSql("((0.00))");
            builder.Property(e => e.Venta).HasColumnType("decimal(18, 2)").HasDefaultValueSql("((0.00))");
            builder.Property(e => e.ImporteVenta).HasColumnType("decimal(18, 2)").HasDefaultValueSql("((0.00))");
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.HasOne(d => d.OperacionesDetalle).WithMany(p => p.OperacionesServicio).HasForeignKey(d => d.OperacionesDetalleId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_OperacionesServicio_OperacionesDetalle");
            builder.HasOne(d => d.Producto).WithMany(p => p.OperacionesServicio).HasForeignKey(d => d.ProductoId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_OperacionesServicio_Producto");
        }
    }
}