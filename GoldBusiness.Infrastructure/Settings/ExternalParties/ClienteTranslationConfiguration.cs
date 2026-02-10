using GoldBusiness.Domain.Entities;
using GoldBusiness.Domain.Translation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.ExternalParties
{
    public class ClienteTranslationConfiguration : IEntityTypeConfiguration<ClienteTranslation>
    {
        public void Configure(EntityTypeBuilder<ClienteTranslation> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => new { e.ClienteId, e.Language }).IsUnique();
            builder.Property(e => e.Language).IsRequired().HasMaxLength(5);
            builder.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.Ignore(e => e.Cliente);
            builder.HasOne<Cliente>().WithMany(c => c.Translations).HasForeignKey(e => e.ClienteId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_ClienteTranslation_Cliente");
        }
    }
}
