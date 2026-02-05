using GoldBusiness.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.Accounting
{
    public class ComprobanteConfiguration : IEntityTypeConfiguration<Comprobante>
    {
        public void Configure(EntityTypeBuilder<Comprobante> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => e.EstablecimientoId);
            builder.Property(e => e.NoComprobante).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Fecha).HasColumnType("datetime");
            builder.Property(e => e.Observaciones).HasMaxLength(1024);
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.Ignore(e => e.Translations);
            builder.Ignore(e => e.Detalles);
            builder.HasOne(d => d.EstablecimientoNavigation).WithMany().HasForeignKey(d => d.EstablecimientoId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Comprobante_Establecimiento");
        }
    }
}
