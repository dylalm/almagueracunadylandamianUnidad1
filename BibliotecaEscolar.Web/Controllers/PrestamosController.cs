using BibliotecaEscolar.Web.Data;
using BibliotecaEscolar.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BibliotecaEscolar.Web.Controllers;

[Authorize]
public class PrestamosController : Controller
{
    private readonly BibliotecaContext _context;

    public PrestamosController(BibliotecaContext context)
    {
        _context = context;
    }

    // Lista general
    public async Task<IActionResult> Index()
    {
        var prestamos = await _context.Prestamos
            .Include(p => p.Usuario)
            .Include(p => p.Libro)
            .ToListAsync();

        return View(prestamos);
    }

    // Mis préstamos
    public async Task<IActionResult> MisPrestamos()
    {
        var idUsuario = int.Parse(
            User.FindFirst("IdUsuario")!.Value);

        var prestamos = await _context.Prestamos
            .Include(p => p.Libro)
            .Where(p => p.IdUsuario == idUsuario)
            .ToListAsync();

        return View(prestamos);
    }

    // Crear préstamo
    public async Task<IActionResult> Create(int idLibro)
    {
        var libro = await _context.Libros
            .FirstOrDefaultAsync(l => l.IdLibro == idLibro);

        if (libro == null)
            return NotFound();

        if (libro.Existencias <= 0)
        {
            TempData["Error"] =
                "No hay existencias disponibles.";

            return RedirectToAction(
                "Details",
                "Libros",
                new { id = idLibro });
        }

        var idUsuario = int.Parse(
            User.FindFirst("IdUsuario")!.Value);

        var prestamo = new Prestamo
        {
            IdUsuario = idUsuario,
            IdLibro = libro.IdLibro,
            FechaPrestamo = DateTime.Now,
            FechaDevolucion = DateTime.Now.AddDays(7),
            Estado = "Activo"
        };

        libro.Existencias--;

        _context.Prestamos.Add(prestamo);

        await _context.SaveChangesAsync();

        TempData["Success"] =
            "Préstamo realizado correctamente.";

        return RedirectToAction(nameof(MisPrestamos));
    }

    // Devolver libro
    public async Task<IActionResult> Devolver(int id)
    {
        var prestamo = await _context.Prestamos
            .Include(p => p.Libro)
            .FirstOrDefaultAsync(p => p.IdPrestamo == id);

        if (prestamo == null)
            return NotFound();

        if (prestamo.Estado == "Devuelto")
        {
            return RedirectToAction(nameof(MisPrestamos));
        }

        prestamo.Estado = "Devuelto";

        if (prestamo.Libro != null)
        {
            prestamo.Libro.Existencias++;
        }

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(MisPrestamos));
    }

    public async Task<IActionResult> Details(int id)
    {
        var prestamo = await _context.Prestamos
            .Include(p => p.Usuario)
            .Include(p => p.Libro)
            .FirstOrDefaultAsync(p => p.IdPrestamo == id);

        if (prestamo == null)
            return NotFound();

        return View(prestamo);
    }
}