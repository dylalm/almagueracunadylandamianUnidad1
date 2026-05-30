using Microsoft.AspNetCore.Mvc;

namespace BibliotecaEscolar.Web.Controllers;

public class ErrorController : Controller
{
    [Route("Error/404")]
    public IActionResult Error404()
    {
        return View();
    }
}