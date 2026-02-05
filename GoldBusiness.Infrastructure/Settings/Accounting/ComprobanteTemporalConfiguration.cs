using GoldBusiness.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.Accounting
{
    public class ComprobanteTemporalConfiguration : IEntityTypeConfiguration<ComprobanteTemporal>
    {
        public void Configure(EntityTypeBuilder<ComprobanteTemporal> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => e.EstablecimientoId);
            builder.Property(e => e.CodigoTransaccion).IsRequired().HasMaxLength(10);
            builder.Property(e => e.Transaccion).IsRequired().HasMaxLength(256);
            builder.Property(e => e.NoDocumento).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Fecha).HasColumnType("datetime");
            builder.Property(e => e.Cuenta).IsRequired().HasMaxLength(8);
            builder.Property(e => e.Departamento).HasMaxLength(50);
            builder.Property(e => e.Descripcion).HasMaxLength(512);
            builder.Property(e => e.Debito).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.Credito).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.Parcial).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Ignore(e => e.Translations);
            builder.HasOne(d => d.EstablecimientoNavigation).WithMany().HasForeignKey(d => d.EstablecimientoId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_ComprobanteTemporal_Establecimiento");
        }
    }
}
