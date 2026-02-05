using GoldBusiness.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.AccountPlan
{
    public class SubGrupoCuentaConfiguration : IEntityTypeConfiguration<SubGrupoCuenta>
    {
        public void Configure(EntityTypeBuilder<SubGrupoCuenta> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => e.GrupoCuentaId);
            builder.HasIndex(e => new { e.Codigo, e.GrupoCuentaId, e.Cancelado }).HasDatabaseName("IX_SubGrupoCuenta").IsUnique();
            builder.Property(e => e.Codigo).IsRequired().HasMaxLength(5);
            builder.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.Ignore(e => e.Translations);
            builder.Ignore(e => e.Cuenta);
            builder.HasOne(d => d.GrupoCuenta)
                .WithMany().HasForeignKey(d => d.GrupoCuentaId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_SubGrupoCuenta_GrupoCuenta");
        }
    }
}