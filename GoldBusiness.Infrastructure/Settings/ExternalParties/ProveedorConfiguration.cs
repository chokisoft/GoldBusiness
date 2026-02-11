using GoldBusiness.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.ExternalParties
{
    public class ProveedorConfiguration : IEntityTypeConfiguration<Proveedor>
    {
        public void Configure(EntityTypeBuilder<Proveedor> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => new { e.Codigo, e.Cancelado }).HasDatabaseName("IX_Proveedor").IsUnique();
            builder.Property(e => e.Codigo).IsRequired().HasMaxLength(5);
            builder.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);
            builder.Property(e => e.Nif).HasMaxLength(11);
            builder.Property(e => e.Iban).HasMaxLength(27);
            builder.Property(e => e.BicoSwift).HasMaxLength(11);
            builder.Property(e => e.Iva).HasColumnType("decimal(5, 2)");
            builder.Property(e => e.Direccion).HasMaxLength(256);
            builder.Property(e => e.Municipio).HasMaxLength(50);
            builder.Property(e => e.Provincia).HasMaxLength(50);
            builder.Property(e => e.CodPostal).HasMaxLength(5);
            builder.Property(e => e.Web).HasMaxLength(256);
            builder.Property(e => e.Email1).HasMaxLength(256);
            builder.Property(e => e.Email2).HasMaxLength(256);
            builder.Property(e => e.Telefono1).HasMaxLength(50);
            builder.Property(e => e.Telefono2).HasMaxLength(50);
            builder.Property(e => e.Fax1).HasMaxLength(50);
            builder.Property(e => e.Fax2).HasMaxLength(50);
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.Ignore(e => e.Translations);
            builder.Ignore(e => e.OperacionesEncabezado);
            builder.Ignore(e => e.Producto);
        }
    }
}
