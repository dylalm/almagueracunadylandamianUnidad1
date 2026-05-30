using System.ComponentModel.DataAnnotations;

namespace BibliotecaEscolar.Web.Models;

public class Usuario
{
    [Key]
    public int IdUsuario { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Correo { get; set; } = string.Empty;

    public string Contrasena { get; set; } = string.Empty;

    public string Matricula { get; set; } = string.Empty;

    public string Rol { get; set; } = "Usuario";

    public ICollection<Prestamo>? Prestamos { get; set; }
}   