using GoldBusiness.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.AccountPlan
{
    public class CuentaConfiguration : IEntityTypeConfiguration<Cuenta>
    {
        public void Configure(EntityTypeBuilder<Cuenta> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => e.SubGrupoCuentaId);
            builder.HasIndex(e => new { e.Codigo, e.SubGrupoCuentaId, e.Cancelado }).HasDatabaseName("IX_Cuenta").IsUnique();
            builder.Property(e => e.Codigo).IsRequired().HasMaxLength(8);
            builder.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.Ignore(e => e.Translations);
            builder.Ignore(e => e.LocalidadCuentaInventarioNavigation);
            builder.Ignore(e => e.LocalidadCuentaCostoNavigation);
            builder.Ignore(e => e.LocalidadCuentaVentaNavigation);
            builder.Ignore(e => e.LocalidadCuentaDevolucionNavigation);
            builder.Ignore(e => e.ConfiguracionCuentaPagarNavigation);
            builder.Ignore(e => e.ConfiguracionCuentaCobrarNavigation);
            builder.HasOne(d => d.SubGrupoCuenta).WithMany().HasForeignKey(d => d.SubGrupoCuentaId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Cuenta_SubGrupoCuenta");
        }
    }
}
