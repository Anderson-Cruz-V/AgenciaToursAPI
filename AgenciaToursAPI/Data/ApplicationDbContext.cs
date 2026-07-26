using AgenciaToursAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AgenciaToursAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Pais> Paises { get; set; } = null!;
    public DbSet<Destino> Destinos { get; set; } = null!;
    public DbSet<Tour> Tours { get; set; } = null!;
    public DbSet<CategoriaTour> CategoriaTours { get; set; } = null!;
    public DbSet<Cliente> Clientes { get; set; } = null!;
    public DbSet<Reserva> Reservas { get; set; } = null!;

    public DbSet<GuiaTuristico> GuiasTuristicos { get; set; }

    public DbSet<Transporte> Transportes { get; set; }

    public DbSet<MetodoPago> MetodosPago { get; set; }

    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Tour>()
            .HasOne(tour => tour.Pais)
            .WithMany()
            .HasForeignKey(tour => tour.PaisId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}