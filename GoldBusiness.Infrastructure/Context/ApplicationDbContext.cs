using GoldBusiness.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace GoldBusiness.Infrastructure.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Cuenta> Cuenta { get; set; } = null!;
        public DbSet<SubGrupoCuenta> SubGrupoCuenta { get; set; } = null!;
        public DbSet<GrupoCuenta> GrupoCuenta { get; set; } = null!;
        public DbSet<GrupoCuentaTranslation> GrupoCuentaTranslation { get; set; } = null!;
        public DbSet<SubGrupoCuentaTranslation> SubGrupoCuentaTranslation { get; set; } = null!;
        public DbSet<CuentaTranslation> CuentaTranslation { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cuenta>(entity =>
            {
                entity.HasIndex(e => e.SubGrupoCuentaId)
                    .HasDatabaseName("IX_Cuenta_SubGrupo");

                entity.HasIndex(e => new { e.Codigo, e.SubGrupoCuentaId, e.Cancelado })
                    .HasDatabaseName("IX_Cuenta")
                    .IsUnique();

                entity.Property(e => e.Codigo)
                    .IsRequired()
                    .HasMaxLength(8);

                entity.Property(e => e.CreadoPor)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");

                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");

                entity.Property(e => e.ModificadoPor)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasOne(d => d.SubGrupoCuenta)
                    .WithMany(p => p.Cuenta)
                    .HasForeignKey(d => d.SubGrupoCuentaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cuenta_SubGrupoCuenta");
            });

            modelBuilder.Entity<GrupoCuenta>(entity =>
            {
                entity.HasIndex(e => new { e.Descripcion, e.Cancelado })
                    .HasDatabaseName("IX_GrupoCuenta")
                    .IsUnique();

                entity.Property(e => e.Codigo)
                    .IsRequired()
                    .HasMaxLength(2);

                entity.Property(e => e.CreadoPor)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");

                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");

                entity.Property(e => e.ModificadoPor)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<SubGrupoCuenta>(entity =>
            {
                entity.HasIndex(e => e.GrupoCuentaId);

                entity.HasIndex(e => new { e.Codigo, e.GrupoCuentaId, e.Cancelado })
                    .HasDatabaseName("IX_SubGrupoCuenta")
                    .IsUnique();

                entity.Property(e => e.Codigo)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.CreadoPor)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.FechaHoraCreado).HasColumnType("datetime");

                entity.Property(e => e.FechaHoraModificado).HasColumnType("datetime");

                entity.Property(e => e.ModificadoPor)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasOne(d => d.GrupoCuenta)
                    .WithMany(p => p.SubGrupoCuenta)
                    .HasForeignKey(d => d.GrupoCuentaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubGrupoCuenta_GrupoCuenta");
            });

            // Traducciones - mappings
            modelBuilder.Entity<GrupoCuentaTranslation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.GrupoCuentaId, e.Language }).IsUnique();
                entity.Property(e => e.Language).IsRequired().HasMaxLength(5);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);

                entity.HasOne(e => e.GrupoCuenta)
                      .WithMany(g => g.Translations)
                      .HasForeignKey(e => e.GrupoCuentaId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<SubGrupoCuentaTranslation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.SubGrupoCuentaId, e.Language }).IsUnique();
                entity.Property(e => e.Language).IsRequired().HasMaxLength(5);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);

                entity.HasOne(e => e.SubGrupoCuenta)
                      .WithMany(s => s.Translations)
                      .HasForeignKey(e => e.SubGrupoCuentaId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CuentaTranslation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.CuentaId, e.Language }).IsUnique();
                entity.Property(e => e.Language).IsRequired().HasMaxLength(5);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(256);

                entity.HasOne(e => e.Cuenta)
                      .WithMany(c => c.Translations)
                      .HasForeignKey(e => e.CuentaId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}