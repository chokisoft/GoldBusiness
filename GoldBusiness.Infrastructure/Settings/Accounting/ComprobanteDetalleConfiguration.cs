using GoldBusiness.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.Accounting
{
    public class ComprobanteDetalleConfiguration : IEntityTypeConfiguration<ComprobanteDetalle>
    {
        public void Configure(EntityTypeBuilder<ComprobanteDetalle> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => e.ComprobanteId);
            builder.HasIndex(e => e.CuentaId);
            builder.Property(e => e.Departamento).HasMaxLength(50);
            builder.Property(e => e.Debito).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.Credito).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.Parcial).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.Nota).HasMaxLength(512);
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.Ignore(e => e.Translations);
            builder.HasOne(d => d.ComprobanteNavigation).WithMany(p => p.Detalles).HasForeignKey(d => d.ComprobanteId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_ComprobanteDetalle_Comprobante");
            builder.HasOne(d => d.CuentaNavigation).WithMany().HasForeignKey(d => d.CuentaId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_ComprobanteDetalle_Cuenta");
        }
    }
}
