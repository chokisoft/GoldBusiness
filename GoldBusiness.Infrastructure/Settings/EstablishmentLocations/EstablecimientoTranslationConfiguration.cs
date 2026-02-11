using GoldBusiness.Domain.Entities;
using GoldBusiness.Domain.Translation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.EstablishmentLocations
{
    public class EstablecimientoTranslationConfiguration : IEntityTypeConfiguration<EstablecimientoTranslation>
    {
        public void Configure(EntityTypeBuilder<EstablecimientoTranslation> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => new { e.EstablecimientoId, e.Language }).IsUnique();
            builder.Property(e => e.Language).IsRequired().HasMaxLength(5);
            builder.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.HasOne(t => t.Establecimiento).WithMany(e => e.Translations).HasForeignKey(e => e.EstablecimientoId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_EstablecimientoTranslation_Establecimiento");
        }
    }
}
