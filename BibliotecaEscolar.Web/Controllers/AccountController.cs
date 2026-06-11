using Microsoft.AspNetCore.Mvc;
using BibliotecaEscolar.Web.Data;
using BibliotecaEscolar.Web.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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

    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var usuario = _context.Usuarios.FirstOrDefault(u =>
            u.Correo == model.Correo &&
            u.Contrasena == model.Contrasena);

        if (usuario == null)
        {
            ModelState.AddModelError(
                "",
                "Correo o contraseña incorrectos.");

            return View(model);
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuario.Nombre),
            new Claim(ClaimTypes.Email, usuario.Correo),
            new Claim(ClaimTypes.Role, usuario.Rol),
            new Claim("IdUsuario", usuario.IdUsuario.ToString())
        };

        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal);

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("Login");
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

    [AllowAnonymous]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public IActionResult ForgotPassword(
        ForgotPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var usuario = _context.Usuarios.FirstOrDefault(u =>
            u.Correo == model.Correo &&
            u.Matricula == model.Matricula);

        if (usuario == null)
        {
            ModelState.AddModelError(
                "",
                "Los datos no coinciden.");

            return View(model);
        }

        TempData["Contrasena"] =
            usuario.Contrasena;

        return RedirectToAction(
            "MostrarContrasena");
    }

    [AllowAnonymous]
    public IActionResult MostrarContrasena()
    {
        return View();
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