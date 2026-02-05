using GoldBusiness.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.Inventory
{
    public class SaldoAnteriorConfiguration : IEntityTypeConfiguration<SaldoAnterior>
    {
        public void Configure(EntityTypeBuilder<SaldoAnterior> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => e.LocalidadId);
            builder.HasIndex(e => e.ProductoId);
            builder.HasIndex(e => new { e.LocalidadId, e.ProductoId, e.Fecha }).HasDatabaseName("IX_SaldoAnterior_Localidad_Producto_Fecha");
            builder.Property(e => e.Existencia).HasColumnType("numeric(18, 3)");
            builder.Property(e => e.ImporteCosto).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.PrecioCosto).HasColumnType("decimal(18, 2)").HasDefaultValueSql("((0.00))");
            builder.Property(e => e.Fecha).HasColumnType("datetime");
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.HasOne(d => d.LocalidadNavigation).WithMany(p => p.SaldosAnteriores).HasForeignKey(d => d.LocalidadId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_SaldoAnterior_Localidad");
            builder.HasOne(d => d.ProductoNavigation).WithMany(p => p.SaldosAnteriores).HasForeignKey(d => d.ProductoId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_SaldoAnterior_Producto");
        }
    }
}
