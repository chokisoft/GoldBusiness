using GoldBusiness.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.AccountPlan
{
    public class GrupoCuentaConfiguration : IEntityTypeConfiguration<GrupoCuenta>
    {
        public void Configure(EntityTypeBuilder<GrupoCuenta> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => new { e.Codigo, e.Cancelado }).HasDatabaseName("IX_GrupoCuenta").IsUnique();
            builder.Property(e => e.Codigo).IsRequired().HasMaxLength(2);
            builder.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.Ignore(e => e.Translations);
            builder.HasMany(e => e.SubGrupoCuenta).WithOne(sg => sg.GrupoCuenta).HasForeignKey(sg => sg.GrupoCuentaId).OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
