using Microsoft.EntityFrameworkCore;
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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
            optionsBuilder.UseSqlServer("Server=DESKTOP-CHB8USB;Database=CargoRoute;Trusted_Connection=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CargoRoute>(entity =>
        {
            entity.HasNoKey();

            entity.ToTable("CargoRoute");

            entity.HasIndex(e => e.CorrelationId, "IX_Route");

            entity.Property(e => e.Address).HasMaxLength(250);

            entity.Property(e => e.CargoId).HasMaxLength(50);

            entity.Property(e => e.CorrelationId).HasMaxLength(50);

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");

            entity.Property(e => e.Route).HasMaxLength(4000);

            entity.Property(e => e.CargoRouteId).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
