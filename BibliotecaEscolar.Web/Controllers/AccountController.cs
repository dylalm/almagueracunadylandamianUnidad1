using Microsoft.AspNetCore.Mvc;
using BibliotecaEscolar.Web.Data;
using BibliotecaEscolar.Web.Models;

namespace BibliotecaEscolar.Web.Controllers;

public class AccountController : Controller
{
    private readonly BibliotecaContext _context;

    public AccountController(BibliotecaContext context)
    {
        _context = context;
    }

    // GET: Account/Register
    public IActionResult Register()
    {
        return View();
    }

    // POST: Account/Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(Usuario usuario)
    {
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

        _context.Add(usuario);

        await _context.SaveChangesAsync();

        TempData["Mensaje"] =
            "Usuario registrado correctamente.";

        return RedirectToAction(nameof(Register));
    }
}