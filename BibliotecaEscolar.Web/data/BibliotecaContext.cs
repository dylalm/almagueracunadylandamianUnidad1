using Microsoft.EntityFrameworkCore;
using BibliotecaEscolar.Web.Models;

namespace BibliotecaEscolar.Web.Data;

public class BibliotecaContext : DbContext
{
    public BibliotecaContext(DbContextOptions<BibliotecaContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }

    public DbSet<Libro> Libros { get; set; }

    public DbSet<Categoria> Categorias { get; set; }

    public DbSet<Prestamo> Prestamos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Correo)
            .IsUnique();

        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Matricula)
            .IsUnique();
    }
}