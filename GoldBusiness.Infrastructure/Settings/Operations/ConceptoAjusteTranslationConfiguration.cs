using GoldBusiness.Domain.Entities;
using GoldBusiness.Domain.Translation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.Operations
{
    public class ConceptoAjusteTranslationConfiguration : IEntityTypeConfiguration<ConceptoAjusteTranslation>
    {
        public void Configure(EntityTypeBuilder<ConceptoAjusteTranslation> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => new { e.ConceptoAjusteId, e.Language }).IsUnique();
            builder.Property(e => e.Language).IsRequired().HasMaxLength(5);
            builder.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.Ignore(e => e.ConceptoAjuste);
            builder.HasOne<ConceptoAjuste>().WithMany(g => g.Translations).HasForeignKey(e => e.ConceptoAjusteId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_ConceptoAjusteTranslation_ConceptoAjuste");
        }
    }
}
