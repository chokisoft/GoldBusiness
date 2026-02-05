using GoldBusiness.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.Cashier
{
    public class IdTurnoConfiguration : IEntityTypeConfiguration<IdTurno>
    {
        public void Configure(EntityTypeBuilder<IdTurno> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Fecha).HasColumnType("datetime");
            builder.Property(e => e.Cajero).IsRequired().HasMaxLength(256);
            builder.Property(e => e.Inicio).HasColumnType("datetime");
            builder.Property(e => e.Fondo).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.Extraccion).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.Cierre).HasColumnType("datetime");
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.Ignore(e => e.CajasRegistradoras);
        }
    }
}
