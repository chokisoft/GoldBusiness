using GoldBusiness.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.Commons
{
    public class PaisConfiguration : IEntityTypeConfiguration<Pais>
    {
        public void Configure(EntityTypeBuilder<Pais> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => new { e.Codigo, e.Cancelado }).IsUnique().HasDatabaseName("IX_Pais");
            builder.HasIndex(e => e.CodigoAlpha2).HasDatabaseName("IX_Alpha2");
            builder.HasIndex(e => e.CodigoTelefono).HasDatabaseName("IX_Telefono");
            builder.Property(e => e.Codigo).IsRequired().HasMaxLength(3).IsFixedLength();
            builder.Property(e => e.CodigoAlpha2).IsRequired().HasMaxLength(2).IsFixedLength();
            builder.Property(e => e.CodigoTelefono).IsRequired().HasMaxLength(5);
            builder.Property(e => e.Descripcion).IsRequired().HasMaxLength(100);
            builder.Property(e => e.RegexTelefono).IsRequired().HasMaxLength(200);
            builder.Property(e => e.FormatoTelefono).IsRequired().HasMaxLength(50);
            builder.Property(e => e.FormatoEjemplo).IsRequired().HasMaxLength(20);
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.Ignore(e => e.Translations);
        }
    }
}