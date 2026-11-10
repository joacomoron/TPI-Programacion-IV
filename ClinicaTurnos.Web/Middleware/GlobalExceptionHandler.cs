using ClinicaTurnos.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicaTurnos.Web.Middleware
{
    // Atrapa cualquier excepción que no se manejó y la convierte en una respuesta HTTP prolija.
    // Así los controllers no necesitan try/catch: los servicios lanzan excepciones y acá se traducen.
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var (status, titulo, detalle) = exception switch
            {
                NotFoundException => (StatusCodes.Status404NotFound, "Recurso no encontrado", exception.Message),
                BusinessRuleException => (StatusCodes.Status400BadRequest, "Regla de negocio no cumplida", exception.Message),
                // Ej: borrar un médico que tiene turnos (la FK con Restrict lo impide)
                DbUpdateException => (StatusCodes.Status409Conflict, "Conflicto con datos relacionados",
                    "No se puede completar la operación porque el registro tiene datos asociados"),
                _ => (StatusCodes.Status500InternalServerError, "Error interno del servidor", "Ocurrió un error inesperado")
            };

            if (status == StatusCodes.Status500InternalServerError)
                _logger.LogError(exception, "Error no controlado");

            httpContext.Response.StatusCode = status;
            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = status,
                Title = titulo,
                Detail = detalle
            }, cancellationToken);

            return true; // true = "ya la manejé, no sigas"
        }
    }
}
