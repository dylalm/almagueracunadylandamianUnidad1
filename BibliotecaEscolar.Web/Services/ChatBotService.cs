namespace BibliotecaEscolar.Web.Services;

public class ChatBotService
{
    public string Responder(string pregunta)
    {
        pregunta = pregunta.ToLower();

        if (pregunta.Contains("prestamo"))
        {
            return "Para solicitar un préstamo debe iniciar sesión y seleccionar un libro disponible.";
        }

        if (pregunta.Contains("registro"))
        {
            return "Puede registrarse desde la opción Registrarse del menú principal.";
        }

        if (pregunta.Contains("login") ||
            pregunta.Contains("sesion"))
        {
            return "Puede iniciar sesión con su correo y contraseña.";
        }

        if (pregunta.Contains("libro"))
        {
            return "Los libros disponibles pueden consultarse en el catálogo.";
        }

        if (pregunta.Contains("devolver"))
        {
            return "Para devolver un libro debe acudir a la sección de préstamos.";
        }

        if (pregunta.Contains("horario"))
        {
            return "La biblioteca está disponible de lunes a viernes.";
        }

        return "Lo siento, no encontré una respuesta para esa pregunta. Intente reformularla.";
    }
}