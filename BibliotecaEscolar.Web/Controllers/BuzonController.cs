using BibliotecaEscolar.Web.Data;
using BibliotecaEscolar.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaEscolar.Web.Controllers;

[Authorize]
public class BuzonController : Controller
{
    private readonly BibliotecaContext _context;

    public BuzonController(BibliotecaContext context)
    {
        _context = context;
    }

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

        mensaje.FechaEnvio = DateTime.Now;

        _context.Mensajes.Add(mensaje);

        await _context.SaveChangesAsync();

        TempData["Mensaje"] =
            "Mensaje enviado correctamente.";

        return RedirectToAction(nameof(Create));
    }

    public IActionResult Index()
    {
        return View(_context.Mensajes
            .OrderByDescending(m => m.FechaEnvio)
            .ToList());
    }
}