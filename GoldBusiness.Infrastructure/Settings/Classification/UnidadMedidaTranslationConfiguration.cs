using GoldBusiness.Domain.Entities;
using GoldBusiness.Domain.Translation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.Classification
{
    public class UnidadMedidaTranslationConfiguration : IEntityTypeConfiguration<UnidadMedidaTranslation>
    {
        public void Configure(EntityTypeBuilder<UnidadMedidaTranslation> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => new { e.UnidadMedidaId, e.Language }).IsUnique();
            builder.Property(e => e.Language).IsRequired().HasMaxLength(5);
            builder.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.Ignore(e => e.UnidadMedida);
            builder.HasOne<UnidadMedida>().WithMany().HasForeignKey(e => e.UnidadMedidaId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_UnidadMedidaTranslation_UnidadMedida");
        }
    }
}
