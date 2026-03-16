using GoldBusiness.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.EstablishmentLocations
{
    public class EstablecimientoConfiguration : IEntityTypeConfiguration<Establecimiento>
    {
        public void Configure(EntityTypeBuilder<Establecimiento> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => new { e.Codigo, e.NegocioId, e.Cancelado }).HasDatabaseName("IX_Establecimiento").IsUnique();
            
            builder.Property(e => e.Activo).HasDefaultValue(true).HasSentinel(false);
            builder.Property(e => e.Codigo).IsRequired().HasMaxLength(6);
            builder.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
            builder.Property(e => e.Direccion).HasMaxLength(256);
            builder.Property(e => e.Telefono).HasMaxLength(50);
            
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            
            // Relaciones
            builder.HasOne(d => d.Negocio)
                .WithMany()
                .HasForeignKey(d => d.NegocioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Establecimiento_Configuracion");

            builder.HasOne(d => d.Pais)
                .WithMany()
                .HasForeignKey(d => d.PaisId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Establecimiento_Pais");

            builder.HasOne(d => d.Provincia)
                .WithMany()
                .HasForeignKey(d => d.ProvinciaId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Establecimiento_Provincia");

            builder.HasOne(d => d.Municipio)
                .WithMany()
                .HasForeignKey(d => d.MunicipioId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Establecimiento_Municipio");

            builder.HasOne(d => d.CodigoPostal)
                .WithMany()
                .HasForeignKey(d => d.CodigoPostalId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Establecimiento_CodigoPostal");

            builder.Ignore(e => e.Translations);
            builder.Ignore(e => e.Comprobantes);
            builder.Ignore(e => e.ComprobantesTemporales);
            builder.Ignore(e => e.CuentasCobrarPagar);
            builder.Ignore(e => e.EstadosCuenta);
            builder.Ignore(e => e.Localidades);
            builder.Ignore(e => e.OperacionesEncabezado);
            builder.Ignore(e => e.Producto);
        }
    }
}
