using Gimnasio.Entidades;
using Microsoft.EntityFrameworkCore;
namespace Gimnasio.Datos;
public partial class GimnasioContext : DbContext
{
    public GimnasioContext(DbContextOptions<GimnasioContext> options)
        : base(options)
    {
    }
    public virtual DbSet<Plan> Planes { get; set; }
    public virtual DbSet<Socio> Socios { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Plan>(entity =>
        {
            entity.ToTable("Planes");
            entity.HasKey(e => e.PlanId);
            entity.HasIndex(e => e.Nombre, "UQ_Planes_Nombre").IsUnique();
            entity.Property(e => e.Nombre).HasMaxLength(50);
            entity.Property(e => e.Precio).HasColumnType("decimal(8, 2)");
        });
        modelBuilder.Entity<Socio>(entity =>
        {
            entity.ToTable("Socios");
            entity.HasIndex(e => e.PlanId, "IX_Socios_PlanId");
            entity.HasIndex(e => e.Dni, "UQ_Socios_Dni").IsUnique();
            entity.Property(e => e.Dni)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Nombres).HasMaxLength(100);
            entity.HasOne(d => d.Plan).WithMany(p => p.Socios)
                .HasForeignKey(d => d.PlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Socios_Planes");
        });
        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
