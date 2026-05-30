using System.ComponentModel.DataAnnotations;

namespace BibliotecaEscolar.Web.Models;

public class Prestamo
{
    [Key]
    public int IdPrestamo { get; set; }

    public int IdUsuario { get; set; }

    public Usuario? Usuario { get; set; }

    public int IdLibro { get; set; }

    public Libro? Libro { get; set; }

    public DateTime FechaPrestamo { get; set; }

    public DateTime? FechaDevolucion { get; set; }

    public string Estado { get; set; } = "Activo";
}