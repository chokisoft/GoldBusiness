using GoldBusiness.Domain.Entities;
using GoldBusiness.Domain.Translation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.Accounting
{
    public class ComprobanteDetalleTranslationConfiguration : IEntityTypeConfiguration<ComprobanteDetalleTranslation>
    {
        public void Configure(EntityTypeBuilder<ComprobanteDetalleTranslation> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => new { e.ComprobanteDetalleId, e.Language }).IsUnique();
            builder.Property(e => e.Language).IsRequired().HasMaxLength(5);
            builder.Property(e => e.Nota).HasMaxLength(512);
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.Ignore(e => e.ComprobanteDetalle);
            builder.HasOne<ComprobanteDetalle>().WithMany().HasForeignKey(e => e.ComprobanteDetalleId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_ComprobanteDetalleTranslation_ComprobanteDetalle");
        }
    }
}
