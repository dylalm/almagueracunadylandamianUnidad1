using System.ComponentModel.DataAnnotations;

namespace BibliotecaEscolar.Web.Models;

public class Usuario
{
    [Key]
    public int IdUsuario { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio")]
    [EmailAddress(ErrorMessage = "Ingrese un correo válido")]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [StringLength(100, MinimumLength = 8,
        ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
    public string Contrasena { get; set; } = string.Empty;

    [Required(ErrorMessage = "La matrícula es obligatoria")]
    [StringLength(20)]
    public string Matricula { get; set; } = string.Empty;

    public string Rol { get; set; } = "Usuario";
    
    public ICollection<Mensaje>? Mensajes { get; set; }

    public ICollection<Prestamo>? Prestamos { get; set; }
}
