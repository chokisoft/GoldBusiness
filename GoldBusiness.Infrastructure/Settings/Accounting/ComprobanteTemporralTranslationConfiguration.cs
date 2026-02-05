using GoldBusiness.Domain.Entities;
using GoldBusiness.Domain.Translation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.Accounting
{
    public class ComprobanteTemporalTranslationConfiguration : IEntityTypeConfiguration<ComprobanteTemporalTranslation>
    {
        public void Configure(EntityTypeBuilder<ComprobanteTemporalTranslation> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => new { e.ComprobanteTemporalId, e.Language }).IsUnique();
            builder.Property(e => e.Language).IsRequired().HasMaxLength(5);
            builder.Property(e => e.Descripcion).HasMaxLength(512);
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.Ignore(e => e.ComprobanteTemporal);
            builder.HasOne<ComprobanteTemporal>().WithMany().HasForeignKey(e => e.ComprobanteTemporalId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_ComprobanteTemporalTranslation_ComprobanteTemporal");
        }
    }
}
