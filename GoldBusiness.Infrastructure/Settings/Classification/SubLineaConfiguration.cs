using GoldBusiness.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.Classification
{
    public class SubLineaConfiguration : IEntityTypeConfiguration<SubLinea>
    {
        public void Configure(EntityTypeBuilder<SubLinea> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => e.LineaId);
            builder.HasIndex(e => new { e.Codigo, e.LineaId, e.Cancelado }).HasDatabaseName("IX_SubLinea").IsUnique();
            builder.Property(e => e.Codigo).IsRequired().HasMaxLength(5);
            builder.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.Ignore(e => e.Translations);
            builder.Ignore(e => e.Producto);
            builder.HasOne(d => d.Linea).WithMany(p => p.SubLinea).HasForeignKey(d => d.LineaId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_SubLinea_Linea");
        }
    }
}
