using GoldBusiness.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.Commons
{
    public class ProvinciaConfiguration : IEntityTypeConfiguration<Provincia>
    {
        public void Configure(EntityTypeBuilder<Provincia> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => new { e.PaisId, e.Codigo, e.Cancelado }).IsUnique().HasDatabaseName("IX_Provincia");
            builder.HasIndex(e => e.PaisId).HasDatabaseName("IX_Provincia_PaisId");
            builder.Property(e => e.Codigo).IsRequired().HasMaxLength(20);
            builder.Property(e => e.Descripcion).IsRequired().HasMaxLength(150);
            builder.Property(e => e.Cancelado).IsRequired().HasDefaultValue(false);
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.HasOne(e => e.Pais).WithMany().HasForeignKey(e => e.PaisId).OnDelete(DeleteBehavior.Restrict);
            builder.Ignore(e => e.Translations);
        }
    }
}