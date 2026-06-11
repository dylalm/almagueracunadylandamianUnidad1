using System.ComponentModel.DataAnnotations;

namespace BibliotecaEscolar.Web.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "El correo es obligatorio")]
    [EmailAddress(ErrorMessage = "Ingrese un correo válido")]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    public string Contrasena { get; set; } = string.Empty;
}