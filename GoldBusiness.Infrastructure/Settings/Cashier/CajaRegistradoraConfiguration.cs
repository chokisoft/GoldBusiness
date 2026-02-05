using GoldBusiness.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.Cashier
{
    public class CajaRegistradoraConfiguration : IEntityTypeConfiguration<CajaRegistradora>
    {
        public void Configure(EntityTypeBuilder<CajaRegistradora> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => e.IdTurnoId);
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.Ignore(e => e.Detalles);
            builder.HasOne(d => d.IdTurnoNavigation).WithMany().HasForeignKey(d => d.IdTurnoId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CajaRegistradora_IdTurno");
        }
    }
}
