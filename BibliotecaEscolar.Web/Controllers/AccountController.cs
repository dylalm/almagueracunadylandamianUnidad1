using Microsoft.AspNetCore.Mvc;
using BibliotecaEscolar.Web.Data;
using BibliotecaEscolar.Web.Models;
using System.Text.Json;

namespace BibliotecaEscolar.Web.Controllers;

public class AccountController : Controller
{
    private readonly BibliotecaContext _context;
    private readonly IConfiguration _configuration;

    public AccountController(
        BibliotecaContext context,
        IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    private async Task<bool> ValidarReCaptcha(string token)
    {
        var secretKey =
            _configuration["GoogleReCaptcha:SecretKey"];

        using var client = new HttpClient();

        var response = await client.PostAsync(
            $"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={token}",
            null);

        var json = await response.Content.ReadAsStringAsync();

        using var document = JsonDocument.Parse(json);

        return document.RootElement
            .GetProperty("success")
            .GetBoolean();
    }

    // GET: Account/Register
    public IActionResult Register()
    {
        ViewBag.SiteKey =
            _configuration["GoogleReCaptcha:SiteKey"];

        return View();
    }

    // POST: Account/Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(Usuario usuario)
    {
        ViewBag.SiteKey =
            _configuration["GoogleReCaptcha:SiteKey"];

        var captchaResponse =
            Request.Form["g-recaptcha-response"];

        if (string.IsNullOrEmpty(captchaResponse))
        {
            ModelState.AddModelError(
                "",
                "Debe completar el reCAPTCHA.");

            return View(usuario);
        }

        bool captchaValido =
            await ValidarReCaptcha(captchaResponse!);

        if (!captchaValido)
        {
            ModelState.AddModelError(
                "",
                "No se pudo validar el reCAPTCHA.");

            return View(usuario);
        }

        if (!ModelState.IsValid)
        {
            return View(usuario);
        }

        bool correoExiste = _context.Usuarios
            .Any(u => u.Correo == usuario.Correo);

        if (correoExiste)
        {
            ModelState.AddModelError(
                "Correo",
                "El correo ya está registrado.");

            return View(usuario);
        }

        bool matriculaExiste = _context.Usuarios
            .Any(u => u.Matricula == usuario.Matricula);

        if (matriculaExiste)
        {
            ModelState.AddModelError(
                "Matricula",
                "La matrícula ya está registrada.");

            return View(usuario);
        }

        usuario.Rol = "Usuario";

        _context.Usuarios.Add(usuario);

        await _context.SaveChangesAsync();

        TempData["Mensaje"] =
            "Usuario registrado correctamente.";

        return RedirectToAction(nameof(Register));
    }
}