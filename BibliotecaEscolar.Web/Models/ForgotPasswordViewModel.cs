using System.ComponentModel.DataAnnotations;

namespace BibliotecaEscolar.Web.Models;

public class ForgotPasswordViewModel
{
    [Required]
    [EmailAddress]
    public string Correo { get; set; } = string.Empty;

    [Required]
    public string Matricula { get; set; } = string.Empty;
}