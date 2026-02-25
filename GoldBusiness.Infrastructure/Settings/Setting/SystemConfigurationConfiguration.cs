using GoldBusiness.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.Setting
{
    public class SystemConfigurationConfiguration : IEntityTypeConfiguration<SystemConfiguration>
    {
        public void Configure(EntityTypeBuilder<SystemConfiguration> builder)
        {
             builder.HasKey(e => e.Id);
            builder.Property(e => e.CodigoSistema).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Licencia).IsRequired().HasMaxLength(100);
            builder.Property(e => e.NombreNegocio).IsRequired().HasMaxLength(256);
            builder.Property(e => e.Direccion).HasMaxLength(512);
            builder.HasOne(x => x.Pais).WithMany().HasForeignKey(x => x.PaisId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Provincia).WithMany().HasForeignKey(x => x.ProvinciaId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Municipio).WithMany().HasForeignKey(x => x.MunicipioId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.CodigoPostal).WithMany().HasForeignKey(x => x.CodigoPostalId).OnDelete(DeleteBehavior.Restrict);
            builder.Property(e => e.Imagen).HasMaxLength(500);
            builder.Property(e => e.Web).HasMaxLength(256);
            builder.Property(e => e.Email).HasMaxLength(256);
            builder.Property(e => e.Telefono).HasMaxLength(20);
            builder.Property(e => e.CuentaPagarId).IsRequired(false);
            builder.Property(e => e.CuentaCobrarId).IsRequired(false);
            builder.Property(e => e.Caducidad).HasColumnType("datetime");
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.Ignore(e => e.Translations);
            builder.Ignore(e => e.Establecimiento);
            builder.HasOne(d => d.CuentaPagar).WithMany(p => p.ConfiguracionCuentaPagar).HasForeignKey(d => d.CuentaPagarId).OnDelete(DeleteBehavior.Restrict).IsRequired(false).HasConstraintName("FK_Configuracion_CuentaPagar");
            builder.HasOne(d => d.CuentaCobrar).WithMany(p => p.ConfiguracionCuentaCobrar).HasForeignKey(d => d.CuentaCobrarId).OnDelete(DeleteBehavior.Restrict).IsRequired(false).HasConstraintName("FK_Configuracion_CuentaCobrar");
        }
    }
}