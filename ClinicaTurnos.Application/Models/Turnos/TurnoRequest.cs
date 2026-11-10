using System.ComponentModel.DataAnnotations;

namespace ClinicaTurnos.Application.Models.Turnos
{
    public class TurnoRequest
    {
        [Required]
        public DateOnly Fecha { get; set; }   // JSON: "2026-12-01"

        [Required]
        public TimeOnly Hora { get; set; }    // JSON: "09:30:00"

        [Range(1, int.MaxValue)]
        public int PacienteId { get; set; }

        [Range(1, int.MaxValue)]
        public int MedicoId { get; set; }

        [Range(1, int.MaxValue)]
        public int ConsultorioId { get; set; }
    }
}
