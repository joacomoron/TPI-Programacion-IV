using System.Net.Http.Json;
using System.Text.Json;
using ClinicaTurnos.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ClinicaTurnos.Infrastructure.ExternalServices
{
    // Typed client: el HttpClient lo crea y configura IHttpClientFactory (ver Program.cs).
    // Consume la API pública de ArgentinaDatos: GET feriados/{año}
    public class FeriadoService : IFeriadoService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<FeriadoService> _logger;

        public FeriadoService(HttpClient httpClient, ILogger<FeriadoService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<bool> EsFeriadoAsync(DateOnly fecha)
        {
            try
            {
                var feriados = await _httpClient.GetFromJsonAsync<List<FeriadoResponse>>($"feriados/{fecha.Year}");
                var fechaTexto = fecha.ToString("yyyy-MM-dd");

                return feriados?.Any(f => f.Fecha == fechaTexto) ?? false;
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException)
            {
                // Si la API externa está caída no frenamos las reservas: lo registramos y seguimos.
                _logger.LogWarning(ex, "No se pudo consultar la API de feriados para {Fecha}", fecha);
                return false;
            }
        }

        // Forma del JSON que devuelve la API: { "fecha": "2026-12-25", "tipo": "inamovible", "nombre": "Navidad" }
        private record FeriadoResponse(string Fecha, string Tipo, string Nombre);
    }
}
