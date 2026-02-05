using GoldBusiness.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.Accounting
{
    public class CuentaCobrarPagarConfiguration : IEntityTypeConfiguration<CuentaCobrarPagar>
    {
        public void Configure(EntityTypeBuilder<CuentaCobrarPagar> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => e.EstablecimientoId);
            builder.HasIndex(e => e.TransaccionId);
            builder.HasIndex(e => e.ProveedorId);
            builder.HasIndex(e => e.ClienteId);
            builder.Property(e => e.Fecha).HasColumnType("datetime");
            builder.Property(e => e.NoPrimario).HasMaxLength(50);
            builder.Property(e => e.NoDocumento).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Importe).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.PagoEfectivoDepartamento).HasMaxLength(50);
            builder.Property(e => e.PagoEfectivoImporte).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.PagoEfectivoParcialMlc).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.PagoElectronicoDepartamento).HasMaxLength(50);
            builder.Property(e => e.PagoElectronicoImporte).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.PagoElectronicoParcialMlc).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.CobroEfectivoDepartamento).HasMaxLength(50);
            builder.Property(e => e.CobroEfectivoImporte).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.CobroEfectivoParcialMlc).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.CobroElectronicoDepartamento).HasMaxLength(50);
            builder.Property(e => e.CobroElectronicoImporte).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.CobroElectronicoParcialMlc).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.HasOne(d => d.EstablecimientoNavigation).WithMany().HasForeignKey(d => d.EstablecimientoId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CuentaCobrarPagar_Establecimiento");
            builder.HasOne(d => d.TransaccionNavigation).WithMany().HasForeignKey(d => d.TransaccionId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CuentaCobrarPagar_Transaccion");
            builder.HasOne(d => d.ProveedorNavigation).WithMany().HasForeignKey(d => d.ProveedorId).OnDelete(DeleteBehavior.SetNull).HasConstraintName("FK_CuentaCobrarPagar_Proveedor");
            builder.HasOne(d => d.ClienteNavigation).WithMany().HasForeignKey(d => d.ClienteId).OnDelete(DeleteBehavior.SetNull).HasConstraintName("FK_CuentaCobrarPagar_Cliente");
            builder.HasOne(d => d.CuentaPagoEfectivoNavigation).WithMany().HasForeignKey(d => d.CuentaPagoEfectivoId).OnDelete(DeleteBehavior.SetNull).HasConstraintName("FK_CuentaCobrarPagar_CuentaPagoEfectivo");
            builder.HasOne(d => d.CuentaPagoElectronicoNavigation).WithMany().HasForeignKey(d => d.CuentaPagoElectronicoId).OnDelete(DeleteBehavior.SetNull).HasConstraintName("FK_CuentaCobrarPagar_CuentaPagoElectronico");
            builder.HasOne(d => d.CuentaCobroEfectivoNavigation).WithMany().HasForeignKey(d => d.CuentaCobroEfectivoId).OnDelete(DeleteBehavior.SetNull).HasConstraintName("FK_CuentaCobrarPagar_CuentaCobroEfectivo");
            builder.HasOne(d => d.CuentaCobroElectronicoNavigation).WithMany().HasForeignKey(d => d.CuentaCobroElectronicoId).OnDelete(DeleteBehavior.SetNull).HasConstraintName("FK_CuentaCobrarPagar_CuentaCobroElectronico");
        }
    }
}
