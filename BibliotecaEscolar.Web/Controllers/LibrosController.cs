using BibliotecaEscolar.Web.Data;
using BibliotecaEscolar.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaEscolar.Web.Controllers;

public class LibrosController : Controller
{
    private readonly BibliotecaContext _context;

    public LibrosController(BibliotecaContext context)
    {
        _context = context;
    }

    // LISTAR
    public async Task<IActionResult> Index()
    {
        var libros = await _context.Libros
            .Include(l => l.Categoria)
            .ToListAsync();

        return View(libros);
    }

    // FORMULARIO CREAR
    public IActionResult Create()
    {
        return View();
    }

    // GUARDAR
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Libro libro)
    {
        if (ModelState.IsValid)
        {
            _context.Add(libro);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(libro);
    }
}