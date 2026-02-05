using GoldBusiness.Domain.Entities;
using GoldBusiness.Domain.Translation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.ExternalParties
{
    public class ProveedorTranslationConfiguration : IEntityTypeConfiguration<ProveedorTranslation>
    {
        public void Configure(EntityTypeBuilder<ProveedorTranslation> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => new { e.ProveedorId, e.Language }).IsUnique();
            builder.Property(e => e.Language).IsRequired().HasMaxLength(5);
            builder.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.Ignore(e => e.Proveedor);
            builder.HasOne<Proveedor>().WithMany().HasForeignKey(e => e.ProveedorId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_ProveedorTranslation_Proveedor");
        }
    }
}
