using GoldBusiness.Domain.Entities;
using GoldBusiness.Domain.Translation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.Operations
{
    public class OperacionesEncabezadoTranslationConfiguration : IEntityTypeConfiguration<OperacionesEncabezadoTranslation>
    {
        public void Configure(EntityTypeBuilder<OperacionesEncabezadoTranslation> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => new { e.OperacionesEncabezadoId, e.Language }).IsUnique();
            builder.Property(e => e.Language).IsRequired().HasMaxLength(5);
            builder.Property(e => e.Concepto).HasMaxLength(512);
            builder.Property(e => e.Observaciones).HasMaxLength(1024);
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.Ignore(e => e.OperacionesEncabezado);
            builder.HasOne<OperacionesEncabezado>().WithMany().HasForeignKey(e => e.OperacionesEncabezadoId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_OperacionesEncabezadoTranslation_OperacionesEncabezado");
        }
    }
}
