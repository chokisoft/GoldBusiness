using GoldBusiness.Domain.Translation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.Commons
{
    public class ProvinciaTranslationConfiguration : IEntityTypeConfiguration<ProvinciaTranslation>
    {
        public void Configure(EntityTypeBuilder<ProvinciaTranslation> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => new { e.ProvinciaId, e.Language }).IsUnique().HasDatabaseName("IX_ProvinciaTranslation");
            builder.Property(e => e.Language).IsRequired().HasMaxLength(10);
            builder.Property(e => e.Descripcion).IsRequired().HasMaxLength(150);
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.HasOne(e => e.Provincia).WithMany(p => p.Translations).HasForeignKey(e => e.ProvinciaId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}