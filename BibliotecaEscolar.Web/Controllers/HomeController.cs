using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BibliotecaEscolar.Web.Models;

namespace BibliotecaEscolar.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Ayuda()
    {
        return View();
    }

    public IActionResult Contacto()
    {
        return View();
    } 

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
