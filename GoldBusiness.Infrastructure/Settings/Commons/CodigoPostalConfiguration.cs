using GoldBusiness.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldBusiness.Infrastructure.Settings.Commons
{
    public class CodigoPostalConfiguration : IEntityTypeConfiguration<CodigoPostal>
    {
        public void Configure(EntityTypeBuilder<CodigoPostal> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => e.MunicipioId);
            builder.HasIndex(e => new { e.MunicipioId, e.Codigo, e.Cancelado }).IsUnique().HasDatabaseName("IX_CodigoPostal_Municipio_Codigo_Cancelado");
            builder.Property(e => e.Codigo).IsRequired().HasMaxLength(25);
            builder.Property(e => e.Cancelado).IsRequired().HasDefaultValue(false);
            builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
            builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
            builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
            builder.HasOne(d => d.Municipio).WithMany(m => m.CodigoPostal).HasForeignKey(d => d.MunicipioId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CodigoPostal_Municipio");
        }
    }
}