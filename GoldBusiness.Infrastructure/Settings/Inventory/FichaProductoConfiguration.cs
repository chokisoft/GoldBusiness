using GoldBusiness.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.Inventory
{
    public class FichaProductoConfiguration : IEntityTypeConfiguration<FichaProducto>
    {
        public void Configure(EntityTypeBuilder<FichaProducto> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => e.ProductoId);
            builder.HasIndex(e => e.ComponenteId);
            builder.HasIndex(e => e.LocalidadId);
            builder.Property(e => e.Cantidad).HasColumnType("numeric(18, 3)");
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.HasOne(d => d.ProductoNavigation).WithMany(p => p.FichaProductoProductoNavigation).HasForeignKey(d => d.ProductoId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_FichaProducto_Producto");
            builder.HasOne(d => d.ComponenteNavigation).WithMany(p => p.FichaProductoCodigoNavigation).HasForeignKey(d => d.ComponenteId).HasPrincipalKey(p => p.Id).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_FichaProducto_Componente");
            builder.HasOne(d => d.LocalidadNavigation).WithMany(p => p.FichaProductos).HasForeignKey(d => d.LocalidadId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_FichaProducto_Localidad");
        }
    }
}
