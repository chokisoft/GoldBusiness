using GoldBusiness.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.Inventory
{
    public class SaldoConfiguration : IEntityTypeConfiguration<Saldo>
    {
        public void Configure(EntityTypeBuilder<Saldo> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => e.LocalidadId);
            builder.HasIndex(e => e.ProductoId);
            builder.HasIndex(e => new { e.LocalidadId, e.ProductoId }).IsUnique().HasDatabaseName("IX_Saldo_Localidad_Producto");
            builder.Property(e => e.Existencia).HasColumnType("numeric(18, 2)");
            builder.Property(e => e.Fecha).HasColumnType("datetime");
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.HasOne(d => d.LocalidadNavigation).WithMany(p => p.Saldos).HasForeignKey(d => d.LocalidadId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Saldo_Localidad");
            builder.HasOne(d => d.ProductoNavigation).WithMany(p => p.Saldos).HasForeignKey(d => d.ProductoId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Saldo_Producto");
        }
    }
}
