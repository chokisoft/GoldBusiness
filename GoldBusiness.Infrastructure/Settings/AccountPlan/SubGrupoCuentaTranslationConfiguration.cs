using GoldBusiness.Domain.Entities;
using GoldBusiness.Domain.Translation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.AccountPlan
{
    public class SubGrupoCuentaTranslationConfiguration : IEntityTypeConfiguration<SubGrupoCuentaTranslation>
    {
        public void Configure(EntityTypeBuilder<SubGrupoCuentaTranslation> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => new { e.SubGrupoCuentaId, e.Language }).IsUnique();
            builder.Property(e => e.Language).IsRequired().HasMaxLength(5);
            builder.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.HasOne(e => e.SubGrupoCuenta).WithMany(s => s.Translations).HasForeignKey(e => e.SubGrupoCuentaId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
