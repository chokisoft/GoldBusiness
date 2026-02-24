using GoldBusiness.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CodigoPostalConfiguration : IEntityTypeConfiguration<CodigoPostal>
{
    public void Configure(EntityTypeBuilder<CodigoPostal> builder)
    {
        builder.HasKey(cp => cp.Id);
        builder.Property(cp => cp.Codigo).IsRequired().HasMaxLength(20);
        builder.Property(e => e.Cancelado).IsRequired().HasDefaultValue(false);
        builder.Property(e => e.CreadoPor).IsRequired().HasMaxLength(256);
        builder.Property(e => e.ModificadoPor).IsRequired().HasMaxLength(256);
        builder.Property(e => e.FechaHoraCreado).HasColumnType("datetime");
        builder.Property(e => e.FechaHoraModificado).HasColumnType("datetime");
        builder.HasOne(cp => cp.Municipio).WithMany(m => m.CodigoPostal).HasForeignKey(cp => cp.MunicipioId).OnDelete(DeleteBehavior.Restrict);
    }
}