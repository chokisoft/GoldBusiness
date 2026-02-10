using GoldBusiness.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.Accounting
{
    public class EstadoCuentaConfiguration : IEntityTypeConfiguration<EstadoCuenta>
    {
        public void Configure(EntityTypeBuilder<EstadoCuenta> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => e.EstablecimientoId);
            builder.HasIndex(e => e.CuentaId);
            builder.Property(e => e.Fecha).HasColumnType("datetime");
            builder.Property(e => e.Debito).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.Credito).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.Saldo).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.Referencia).HasMaxLength(256);
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.HasOne(d => d.Establecimiento).WithMany().HasForeignKey(d => d.EstablecimientoId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_EstadoCuenta_Establecimiento");
            builder.HasOne(d => d.Cuenta).WithMany().HasForeignKey(d => d.CuentaId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_EstadoCuenta_Cuenta");
        }
    }
}
