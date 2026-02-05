using GoldBusiness.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.Setting
{
    public class SystemConfigurationConfiguration : IEntityTypeConfiguration<Domain.Entities.SystemConfiguration>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.SystemConfiguration> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.CodigoSistema).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Licencia).IsRequired().HasMaxLength(100);
            builder.Property(e => e.NombreNegocio).IsRequired().HasMaxLength(256);
            builder.Property(e => e.Direccion).HasMaxLength(512);
            builder.Property(e => e.Municipio).HasMaxLength(128);
            builder.Property(e => e.Provincia).HasMaxLength(128);
            builder.Property(e => e.CodPostal).HasMaxLength(20);
            builder.Property(e => e.Imagen).HasColumnType("image");
            builder.Property(e => e.Web).HasMaxLength(256);
            builder.Property(e => e.Email).HasMaxLength(256);
            builder.Property(e => e.Telefono).HasMaxLength(20);
            builder.Property(e => e.Caducidad).HasColumnType("datetime");
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.Ignore(e => e.Translations);
            builder.Ignore(e => e.Establecimientos);
            builder.HasOne(d => d.CuentaPagarNavigation).WithMany(p => p.ConfiguracionCuentaPagarNavigation).HasForeignKey(d => d.CuentaPagarId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Configuracion_CuentaPagar");
            builder.HasOne(d => d.CuentaCobrarNavigation).WithMany(p => p.ConfiguracionCuentaCobrarNavigation).HasForeignKey(d => d.CuentaCobrarId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Configuracion_CuentaCobrar");
        }
    }
}
