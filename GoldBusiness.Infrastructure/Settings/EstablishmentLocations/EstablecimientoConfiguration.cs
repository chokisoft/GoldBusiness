using GoldBusiness.Domain.Entities;
using GoldBusiness.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.EstablishmentLocations
{
    /// <summary>
    /// Configuración EF Core para Establecimiento.
    /// OPTIMIZADO: Solo campos esenciales.
    /// </summary>
    public class EstablecimientoConfiguration : IEntityTypeConfiguration<Establecimiento>
    {
        public void Configure(EntityTypeBuilder<Establecimiento> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => new { e.Codigo, e.NegocioId, e.Cancelado })
                .HasDatabaseName("IX_Establecimiento")
                .IsUnique();
            
            // Propiedades Básicas
            builder.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasSentinel(false);
            
            builder.Property(e => e.Codigo)
                .IsRequired()
                .HasMaxLength(6);
            
            builder.Property(e => e.Descripcion)
                .IsRequired()
                .HasMaxLength(256);
            
            builder.Property(e => e.Direccion)
                .HasMaxLength(256);
            
            builder.Property(e => e.Telefono)
                .HasMaxLength(50);
            
            // Información Organizacional
            builder.Property(e => e.Tipo)
                .HasConversion<int>()
                .IsRequired()
                .HasDefaultValue(TipoEstablecimiento.Sucursal)
                .HasSentinel(0);
            
            // Control de Estado
            builder.Property(e => e.OperativoActualmente)
                .HasDefaultValue(true);
            
            // Auditoría
            builder.Property(e => e.CreadoPor)
                .IsRequired()
                .HasMaxLength(256);
            
            builder.Property(e => e.FechaHoraCreado)
                .HasColumnType("datetime");
            
            builder.Property(e => e.FechaHoraModificado)
                .HasColumnType("datetime");
            
            builder.Property(e => e.ModificadoPor)
                .IsRequired()
                .HasMaxLength(256);
            
            // ??????????????????????????????????????????????????????????????????
            // ?? RELACIONES
            // ??????????????????????????????????????????????????????????????????
            
            builder.HasOne(d => d.Negocio)
                .WithMany()
                .HasForeignKey(d => d.NegocioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Establecimiento_Configuracion");

            builder.HasOne(d => d.Pais)
                .WithMany()
                .HasForeignKey(d => d.PaisId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Establecimiento_Pais");

            builder.HasOne(d => d.Provincia)
                .WithMany()
                .HasForeignKey(d => d.ProvinciaId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Establecimiento_Provincia");

            builder.HasOne(d => d.Municipio)
                .WithMany()
                .HasForeignKey(d => d.MunicipioId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Establecimiento_Municipio");

            builder.HasOne(d => d.CodigoPostal)
                .WithMany()
                .HasForeignKey(d => d.CodigoPostalId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Establecimiento_CodigoPostal");
        }
    }
}
