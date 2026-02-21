using GoldBusiness.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.Commons
{
    public class MunicipioConfiguration : IEntityTypeConfiguration<Municipio>
    {
        public void Configure(EntityTypeBuilder<Municipio> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => new { e.ProvinciaId, e.Codigo, e.Cancelado }).IsUnique().HasDatabaseName("IX_Municipio");
            builder.HasIndex(e => e.ProvinciaId).HasDatabaseName("IX_Municipio_ProvinciaId");
            builder.Property(e => e.Codigo).IsRequired().HasMaxLength(25);
            builder.Property(e => e.Descripcion).IsRequired().HasMaxLength(150);
            builder.Property(e => e.Cancelado).IsRequired().HasDefaultValue(false);
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.HasOne(e => e.Provincia).WithMany().HasForeignKey(e => e.ProvinciaId).OnDelete(DeleteBehavior.Restrict);
            builder.Ignore(e => e.Translations);
        }
    }
}