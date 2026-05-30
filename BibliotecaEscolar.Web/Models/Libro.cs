using System.ComponentModel.DataAnnotations;

namespace BibliotecaEscolar.Web.Models;

public class Libro
{
    [Key]
    public int IdLibro { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public string Autor { get; set; } = string.Empty;

    public string Editorial { get; set; } = string.Empty;

    public int Anio { get; set; }

    public int Existencias { get; set; }

    public int IdCategoria { get; set; }

    public Categoria? Categoria { get; set; }

    public ICollection<Prestamo>? Prestamos { get; set; }
}
