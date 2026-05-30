using System.ComponentModel.DataAnnotations;

namespace BibliotecaEscolar.Web.Models;

public class Categoria
{
    [Key]
    public int IdCategoria { get; set; }

    public string NombreCategoria { get; set; } = string.Empty;

    public ICollection<Libro>? Libros { get; set; }
}