using GoldBusiness.Domain.Entities;
using GoldBusiness.Domain.Translation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.Classification
{
    public class MonedaTranslationConfiguration : IEntityTypeConfiguration<MonedaTranslation>
    {
        public void Configure(EntityTypeBuilder<MonedaTranslation> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => new { e.MonedaId, e.Language }).IsUnique();
            builder.Property(e => e.Language).IsRequired().HasMaxLength(5);
            builder.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.Ignore(e => e.Moneda);
            builder.HasOne<Moneda>().WithMany(g => g.Translations).HasForeignKey(e => e.MonedaId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_MonedaTranslation_Moneda");
        }
    }
}
