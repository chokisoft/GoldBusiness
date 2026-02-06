using GoldBusiness.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.Classification
{
    public class MonedaConfiguration : IEntityTypeConfiguration<Moneda>
    {
        public void Configure(EntityTypeBuilder<Moneda> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Codigo).IsRequired().HasMaxLength(3);
            builder.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.Ignore(e => e.Translations);
        }
    }
}
