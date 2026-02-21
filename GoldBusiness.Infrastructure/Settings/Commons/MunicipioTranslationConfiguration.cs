using GoldBusiness.Domain.Translation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.Commons
{
    public class MunicipioTranslationConfiguration : IEntityTypeConfiguration<MunicipioTranslation>
    {
        public void Configure(EntityTypeBuilder<MunicipioTranslation> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => new { e.MunicipioId, e.Language }).IsUnique().HasDatabaseName("IX_MunicipioTranslation");
            
            builder.Property(e => e.Language).IsRequired().HasMaxLength(10);
            builder.Property(e => e.Descripcion).IsRequired().HasMaxLength(150);
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.HasOne(e => e.Municipio).WithMany(m => m.Translations).HasForeignKey(e => e.MunicipioId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}