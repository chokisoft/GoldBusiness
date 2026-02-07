using GoldBusiness.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.Operations
{
    public class ConceptoAjusteConfiguration : IEntityTypeConfiguration<ConceptoAjuste>
    {
        public void Configure(EntityTypeBuilder<ConceptoAjuste> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => new { e.Codigo, e.Cancelado }).HasDatabaseName("IX_ConceptoAjuste").IsUnique();
            builder.Property(e => e.Codigo).IsRequired().HasMaxLength(10);
            builder.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.Ignore(e => e.Translations);
            builder.Ignore(e => e.OperacionesEncabezado);
            builder.HasOne(d => d.Cuenta).WithMany().HasForeignKey(d => d.CuentaId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_ConceptoAjuste_Cuenta");
        }
    }
}
