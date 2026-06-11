using BibliotecaEscolar.Web.Data;
using BibliotecaEscolar.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BibliotecaEscolar.Web.Controllers;

[Authorize]
public class BuzonController : Controller
{
    private readonly BibliotecaContext _context;

    public BuzonController(BibliotecaContext context)
    {
        _context = context;
    }

    // Redirección según rol
    public IActionResult Index()
    {
        if (User.IsInRole("Administrador"))
        {
            var mensajes = _context.Mensajes
                .OrderByDescending(m => m.FechaEnvio)
                .ToList();

            return View("IndexAdmin", mensajes);
        }

        return View("IndexUsuario");
    }

    // Formulario para enviar mensaje
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Mensaje mensaje)
    {
        if (!ModelState.IsValid)
        {
            return View(mensaje);
        }

        var idUsuario =
            int.Parse(User.FindFirst("IdUsuario")!.Value);

        var nombre =
            User.Identity?.Name;

        var correo =
            User.FindFirst(ClaimTypes.Email)?.Value;

        mensaje.IdUsuario = idUsuario;
        mensaje.NombreRemitente = nombre ?? "";
        mensaje.CorreoRemitente = correo ?? "";
        mensaje.FechaEnvio = DateTime.Now;
        mensaje.Estado = "Pendiente";

        _context.Mensajes.Add(mensaje);

        await _context.SaveChangesAsync();

        TempData["Mensaje"] =
            "Mensaje enviado correctamente.";

        return RedirectToAction(nameof(MisMensajes));
    }

    // Mensajes del usuario actual
    public IActionResult MisMensajes()
    {
        var idUsuario =
            int.Parse(User.FindFirst("IdUsuario")!.Value);

        var mensajes = _context.Mensajes
            .Where(m => m.IdUsuario == idUsuario)
            .OrderByDescending(m => m.FechaEnvio)
            .ToList();

        return View(mensajes);
    }

    // Detalle para administrador
    [Authorize(Roles = "Administrador")]
    public IActionResult Details(int id)
    {
        var mensaje = _context.Mensajes
            .FirstOrDefault(m => m.IdMensaje == id);

        if (mensaje == null)
        {
            return NotFound();
        }

        return View(mensaje);
    }

    // Responder mensaje
    [Authorize(Roles = "Administrador")]
    public IActionResult Responder(int id)
    {
        var mensaje = _context.Mensajes
            .FirstOrDefault(m => m.IdMensaje == id);

        if (mensaje == null)
        {
            return NotFound();
        }

        return View(mensaje);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Responder(int id, string respuesta)
    {
        var mensaje = _context.Mensajes
            .FirstOrDefault(m => m.IdMensaje == id);

        if (mensaje == null)
        {
            return NotFound();
        }

        mensaje.Respuesta = respuesta;
        mensaje.Estado = "Respondido";

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}