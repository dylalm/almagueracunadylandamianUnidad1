using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BibliotecaEscolar.Web.Data;
using BibliotecaEscolar.Web.Models;

namespace BibliotecaEscolar.Web.Controllers;

public class PrestamosController : Controller
{
    private readonly BibliotecaContext _context;

    public PrestamosController(BibliotecaContext context)
    {
        _context = context;
    }

    // GET: Prestamos
    public async Task<IActionResult> Index()
    {
        var prestamos = _context.Prestamos
            .Include(p => p.Usuario)
            .Include(p => p.Libro);

        return View(await prestamos.ToListAsync());
    }

    // GET: Prestamos/Create
    public IActionResult Create()
    {
        ViewData["IdUsuario"] = new SelectList(
            _context.Usuarios,
            "IdUsuario",
            "Nombre");

        ViewData["IdLibro"] = new SelectList(
            _context.Libros,
            "IdLibro",
            "Titulo");

        return View();
    }

    // POST: Prestamos/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Prestamo prestamo)
    {
        var libro = await _context.Libros
            .FirstOrDefaultAsync(l => l.IdLibro == prestamo.IdLibro);

        if (libro == null)
        {
            ModelState.AddModelError("", "Libro no encontrado.");
        }
        else if (libro.Existencias <= 0)
        {
            ModelState.AddModelError("", "No hay existencias disponibles.");
        }

        if (!ModelState.IsValid)
        {
            ViewData["IdUsuario"] = new SelectList(
                _context.Usuarios,
                "IdUsuario",
                "Nombre",
                prestamo.IdUsuario);

            ViewData["IdLibro"] = new SelectList(
                _context.Libros,
                "IdLibro",
                "Titulo",
                prestamo.IdLibro);

            return View(prestamo);
        }

        prestamo.FechaPrestamo = DateTime.Now;
        prestamo.Estado = "Activo";

        libro!.Existencias--;

        _context.Add(prestamo);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}