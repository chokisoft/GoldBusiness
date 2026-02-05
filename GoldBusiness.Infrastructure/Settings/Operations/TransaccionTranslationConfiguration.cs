using GoldBusiness.Domain.Entities;
using GoldBusiness.Domain.Translation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.Operations
{
    public class TransaccionTranslationConfiguration : IEntityTypeConfiguration<TransaccionTranslation>
    {
        public void Configure(EntityTypeBuilder<TransaccionTranslation> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => new { e.TransaccionId, e.Language }).IsUnique();
            builder.Property(e => e.Language).IsRequired().HasMaxLength(5);
            builder.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.Ignore(e => e.Transaccion);
            builder.HasOne<Transaccion>().WithMany().HasForeignKey(e => e.TransaccionId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_TransaccionTranslation_Transaccion");
        }
    }
}
