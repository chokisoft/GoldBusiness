using GoldBusiness.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.ExternalParties
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => new { e.Codigo, e.Cancelado }).HasDatabaseName("IX_Cliente").IsUnique();
            
            builder.Property(e => e.Codigo).IsRequired().HasMaxLength(8);
            builder.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
            builder.Property(e => e.Nif).HasMaxLength(11);
            builder.Property(e => e.Iban).HasMaxLength(27);
            builder.Property(e => e.BicoSwift).HasMaxLength(11);
            builder.Property(e => e.Iva).HasColumnType("decimal(5, 2)");
            builder.Property(e => e.Direccion).HasMaxLength(256);
            builder.Property(e => e.Telefono1).HasMaxLength(50);
            builder.Property(e => e.Telefono2).HasMaxLength(50);
            builder.Property(e => e.Web).HasMaxLength(256);
            builder.Property(e => e.Email1).HasMaxLength(256);
            builder.Property(e => e.Email2).HasMaxLength(256);
            builder.Property(e => e.Fax1).HasMaxLength(50);
            builder.Property(e => e.Fax2).HasMaxLength(50);
            
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");

            // Relaciones geográficas
            builder.HasOne(d => d.Pais)
                .WithMany()
                .HasForeignKey(d => d.PaisId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Cliente_Pais");

            builder.HasOne(d => d.Provincia)
                .WithMany()
                .HasForeignKey(d => d.ProvinciaId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Cliente_Provincia");

            builder.HasOne(d => d.Municipio)
                .WithMany()
                .HasForeignKey(d => d.MunicipioId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Cliente_Municipio");

            builder.HasOne(d => d.CodigoPostal)
                .WithMany()
                .HasForeignKey(d => d.CodigoPostalId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Cliente_CodigoPostal");

            builder.Ignore(e => e.Translations);
            builder.Ignore(e => e.CuentasCobrarPagar);
            builder.Ignore(e => e.OperacionesEncabezado);
        }
    }
}
