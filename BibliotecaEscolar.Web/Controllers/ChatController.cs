using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BibliotecaEscolar.Web.Models;
using BibliotecaEscolar.Web.Services;

namespace BibliotecaEscolar.Web.Controllers;

[Authorize]
public class ChatController : Controller
{
    private readonly ChatBotService _chatBot;

    public ChatController(ChatBotService chatBot)
    {
        _chatBot = chatBot;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new ChatViewModel());
    }

    [HttpPost]
    public IActionResult Index(ChatViewModel model)
    {
        model.Respuesta =
            _chatBot.Responder(model.Pregunta);

        return View(model);
    }
}
