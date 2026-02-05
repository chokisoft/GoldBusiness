using GoldBusiness.Domain.Entities;
using GoldBusiness.Domain.Translation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.Accounting
{
    public class ComprobanteTranslationConfiguration : IEntityTypeConfiguration<ComprobanteTranslation>
    {
        public void Configure(EntityTypeBuilder<ComprobanteTranslation> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => new { e.ComprobanteId, e.Language }).IsUnique();
            builder.Property(e => e.Language).IsRequired().HasMaxLength(5);
            builder.Property(e => e.Observaciones).HasMaxLength(1024);
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.Ignore(e => e.Comprobante);
            builder.HasOne<Comprobante>().WithMany().HasForeignKey(e => e.ComprobanteId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_ComprobanteTranslation_Comprobante");
        }
    }
}
