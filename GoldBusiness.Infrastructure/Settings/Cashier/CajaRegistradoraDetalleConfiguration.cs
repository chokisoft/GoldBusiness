using GoldBusiness.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.Cashier
{
    public class CajaRegistradoraDetalleConfiguration : IEntityTypeConfiguration<CajaRegistradoraDetalle>
    {
        public void Configure(EntityTypeBuilder<CajaRegistradoraDetalle> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => e.CajaRegistradoraId);
            builder.HasIndex(e => e.LocalidadId);
            builder.HasIndex(e => e.ProductoId);
            builder.Property(e => e.Cantidad).HasColumnType("decimal(18, 3)");
            builder.Property(e => e.Venta).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.ImporteVenta).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.HasOne(d => d.CajaRegistradoraNavigation).WithMany(p => p.Detalles).HasForeignKey(d => d.CajaRegistradoraId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CajaRegistradoraDetalle_CajaRegistradora");
            builder.HasOne(d => d.LocalidadNavigation).WithMany().HasForeignKey(d => d.LocalidadId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CajaRegistradoraDetalle_Localidad");
            builder.HasOne(d => d.ProductoNavigation).WithMany().HasForeignKey(d => d.ProductoId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CajaRegistradoraDetalle_Producto");
        }
    }
}
