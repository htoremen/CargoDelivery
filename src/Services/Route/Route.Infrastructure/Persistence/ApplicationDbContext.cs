﻿using Microsoft.EntityFrameworkCore;
using Route.Domain.Entities;

namespace Route.Infrastructure.Persistence;



public partial class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CargoRoute> CargoRoutes { get; set; } = null!;
    public virtual DbSet<Domain.Entities.Cargo> Cargos { get; set; } = null!;
    public virtual DbSet<Domain.Entities.Route> Routes { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
            optionsBuilder.UseSqlServer("Server=DESKTOP-CHB8USB;Database=CargoRoute;Trusted_Connection=True;");
        }
    }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var result = await base.SaveChangesAsync(cancellationToken);
        return result;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Domain.Entities.Cargo>(entity =>
        {
            entity.ToTable("Cargo");

            entity.Property(e => e.CargoId).HasMaxLength(50);

            entity.Property(e => e.CorrelationId).HasMaxLength(50);

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");

            entity.Property(e => e.EndRouteId).HasMaxLength(50);

            entity.Property(e => e.StartRouteId).HasMaxLength(50);

            //entity.HasOne(d => d.EndRoute)
            //    .WithMany(p => p.CargoEndRoutes)
            //    .HasForeignKey(d => d.EndRouteId)
            //    .HasConstraintName("FK_Cargo_EndRoute");

            //entity.HasOne(d => d.StartRoute)
            //    .WithMany(p => p.CargoStartRoutes)
            //    .HasForeignKey(d => d.StartRouteId)
            //    .HasConstraintName("FK_Cargo_StartRoute");
        });

        modelBuilder.Entity<CargoRoute>(entity =>
        {
            entity.ToTable("CargoRoute");

            entity.HasIndex(e => e.CorrelationId, "IX_Route");

            entity.Property(e => e.CargoRouteId).HasMaxLength(50);

            entity.Property(e => e.Address).HasMaxLength(250);

            entity.Property(e => e.CargoId).HasMaxLength(50);

            entity.Property(e => e.CorrelationId).HasMaxLength(50);

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");

            entity.Property(e => e.Route).HasMaxLength(4000);
        });

        modelBuilder.Entity<Domain.Entities.Route>(entity =>
        {
            entity.ToTable("Route");

            entity.Property(e => e.RouteId).HasMaxLength(50);

            entity.Property(e => e.CorrelationId).HasMaxLength(50);

            entity.Property(e => e.RouteAddress).HasColumnType("ntext");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
