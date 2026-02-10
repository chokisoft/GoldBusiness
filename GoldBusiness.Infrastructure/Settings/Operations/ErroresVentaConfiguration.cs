using GoldBusiness.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.Operations
{
    public class ErroresVentaConfiguration : IEntityTypeConfiguration<ErroresVenta>
    {
        public void Configure(EntityTypeBuilder<ErroresVenta> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => e.OperacionesDetalleId).HasDatabaseName("IX_ErroresVenta_OperacionesDetalle");
            builder.HasIndex(e => e.LocalidadId).HasDatabaseName("IX_ErroresVenta_Localidad");
            builder.HasIndex(e => e.ProductoId).HasDatabaseName("IX_ErroresVenta_Producto");
            builder.Property(e => e.Cantidad).HasColumnType("numeric(18, 2)");
            builder.Property(e => e.Costo).HasColumnType("numeric(18, 2)");
            builder.Property(e => e.ImporteCosto).HasColumnType("numeric(18, 2)");
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.HasOne(d => d.OperacionesDetalle).WithMany().HasForeignKey(d => d.OperacionesDetalleId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_ErroresVenta_OperacionesDetalle");
            builder.HasOne(d => d.Localidad).WithMany(p => p.ErroresVenta).HasForeignKey(d => d.LocalidadId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_ErroresVenta_Localidad");
            builder.HasOne(d => d.Producto).WithMany(p => p.ErroresVenta).HasForeignKey(d => d.ProductoId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_ErroresVenta_Producto");
        }
    }
}
