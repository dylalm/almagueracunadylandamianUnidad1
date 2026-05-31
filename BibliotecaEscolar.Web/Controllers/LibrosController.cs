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

    // GET: Libros
    public async Task<IActionResult> Index()
    {
        var libros = await _context.Libros
            .Include(l => l.Categoria)
            .ToListAsync();

        return View(libros);
    }

    // GET: Libros/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var libro = await _context.Libros
            .Include(l => l.Categoria)
            .FirstOrDefaultAsync(l => l.IdLibro == id);

        if (libro == null)
            return NotFound();

        return View(libro);
    }

    // GET: Libros/Create
    public IActionResult Create()
    {
        ViewBag.Categorias = _context.Categorias.ToList();

        return View();
    }

    // POST: Libros/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Libro libro)
    {
        if (ModelState.IsValid)
        {
            _context.Libros.Add(libro);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        ViewBag.Categorias = _context.Categorias.ToList();

        return View(libro);
    }

    // GET: Libros/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var libro = await _context.Libros.FindAsync(id);

        if (libro == null)
            return NotFound();

        ViewBag.Categorias = _context.Categorias.ToList();

        return View(libro);
    }

    // POST: Libros/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Libro libro)
    {
        if (id != libro.IdLibro)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(libro);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LibroExists(libro.IdLibro))
                    return NotFound();

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewBag.Categorias = _context.Categorias.ToList();

        return View(libro);
    }

    // GET: Libros/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var libro = await _context.Libros
            .Include(l => l.Categoria)
            .FirstOrDefaultAsync(l => l.IdLibro == id);

        if (libro == null)
            return NotFound();

        return View(libro);
    }

    // POST: Libros/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var libro = await _context.Libros.FindAsync(id);

        if (libro != null)
        {
            _context.Libros.Remove(libro);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool LibroExists(int id)
    {
        return _context.Libros.Any(e => e.IdLibro == id);
    }
}