using GoldBusiness.Domain.Entities;
using GoldBusiness.Domain.Translation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.Setting
{
    public class SystemConfigurationTranslationConfiguration : IEntityTypeConfiguration<SystemConfigurationTranslation>
    {
        public void Configure(EntityTypeBuilder<SystemConfigurationTranslation> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => new { e.ConfiguracionId, e.Language }).IsUnique();
            builder.Property(e => e.Language).IsRequired().HasMaxLength(5);
            builder.Property(e => e.NombreNegocio).IsRequired().HasMaxLength(256);
            builder.Property(e => e.Direccion).HasMaxLength(512);
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.Ignore(e => e.Configuracion);
            builder.HasOne<SystemConfiguration>().WithMany(c => c.Translations).HasForeignKey(e => e.ConfiguracionId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_ConfiguracionTranslation_Configuracion");
        }
    }
}
