using GoldBusiness.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.Operations
{
    public class OperacionesEncabezadoConfiguration : IEntityTypeConfiguration<OperacionesEncabezado>
    {
        public void Configure(EntityTypeBuilder<OperacionesEncabezado> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => e.EstablecimientoId);
            builder.HasIndex(e => e.TransaccionId);
            builder.HasIndex(e => e.ProveedorId);
            builder.HasIndex(e => e.ClienteId);
            builder.HasIndex(e => e.ConceptoAjusteId);
            builder.Property(e => e.NoDocumento).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Fecha).HasColumnType("datetime");
            builder.Property(e => e.NoPrimario).HasMaxLength(50);
            builder.Property(e => e.Concepto).HasMaxLength(512);
            builder.Property(e => e.Observaciones).HasMaxLength(1024);
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.Ignore(e => e.Translations);
            builder.Ignore(e => e.OperacionesDetalle);
            builder.HasOne(d => d.EstablecimientoNavigation).WithMany().HasForeignKey(d => d.EstablecimientoId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_OperacionesEncabezado_Establecimiento");
            builder.HasOne(d => d.TransaccionNavigation).WithMany().HasForeignKey(d => d.TransaccionId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_OperacionesEncabezado_Transaccion");
            builder.HasOne(d => d.ProveedorNavigation).WithMany().HasForeignKey(d => d.ProveedorId).OnDelete(DeleteBehavior.SetNull).HasConstraintName("FK_OperacionesEncabezado_Proveedor");
            builder.HasOne(d => d.ClienteNavigation).WithMany().HasForeignKey(d => d.ClienteId).OnDelete(DeleteBehavior.SetNull).HasConstraintName("FK_OperacionesEncabezado_Cliente");
            builder.HasOne(d => d.ConceptoAjusteNavigation).WithMany(p => p.OperacionesEncabezado).HasForeignKey(d => d.ConceptoAjusteId).OnDelete(DeleteBehavior.SetNull).HasConstraintName("FK_OperacionesEncabezado_ConceptoAjuste");
        }
    }
}
