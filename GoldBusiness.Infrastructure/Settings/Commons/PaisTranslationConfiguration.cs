using GoldBusiness.Domain.Translation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.Commons
{
    public class PaisTranslationConfiguration : IEntityTypeConfiguration<PaisTranslation>
    {
        public void Configure(EntityTypeBuilder<PaisTranslation> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => new { e.PaisId, e.Language }).IsUnique().HasDatabaseName("IX_PaisTranslation_PaisId_Idioma");
            builder.Property(e => e.Language).IsRequired().HasMaxLength(10);
            builder.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.HasOne(e => e.Pais).WithMany(g => g.Translations).HasForeignKey(e => e.PaisId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}