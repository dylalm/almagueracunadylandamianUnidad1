using System.ComponentModel.DataAnnotations;

namespace BibliotecaEscolar.Web.Models;

public class Mensaje
{
    [Key]
    public int IdMensaje { get; set; }

    public int IdUsuario { get; set; }

    public Usuario? Usuario { get; set; }

    [Required]
    public string NombreRemitente { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string CorreoRemitente { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Asunto { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string Contenido { get; set; } = string.Empty;

    public DateTime FechaEnvio { get; set; } = DateTime.Now;

    public string Estado { get; set; } = "Pendiente";

    public string? Respuesta { get; set; }
}