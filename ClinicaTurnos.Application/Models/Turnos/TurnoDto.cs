using ClinicaTurnos.Domain.Entities;

namespace ClinicaTurnos.Application.Models.Turnos
{
    public class TurnoDto
    {
        public int Id { get; set; }
        public DateOnly Fecha { get; set; }
        public TimeOnly Hora { get; set; }
        public string Estado { get; set; } = string.Empty;

        public int PacienteId { get; set; }
        public string Paciente { get; set; } = string.Empty;

        public int MedicoId { get; set; }
        public string Medico { get; set; } = string.Empty;
        public string Especialidad { get; set; } = string.Empty;

        public int ConsultorioId { get; set; }
        public int NroConsultorio { get; set; }

        public bool Pagado { get; set; }

        // Necesita que el turno venga con sus Include (ver TurnoRepository.ConDetalles)
        public static TurnoDto FromEntity(Turno t) => new()
        {
            Id = t.Id,
            Fecha = t.Fecha,
            Hora = t.Hora,
            Estado = t.Estado.ToString(),
            PacienteId = t.PacienteId,
            Paciente = t.Paciente is null ? string.Empty : $"{t.Paciente.Nombre} {t.Paciente.Apellido}",
            MedicoId = t.MedicoId,
            Medico = t.Medico is null ? string.Empty : $"{t.Medico.Nombre} {t.Medico.Apellido}",
            Especialidad = t.Medico?.Especialidad ?? string.Empty,
            ConsultorioId = t.ConsultorioId,
            NroConsultorio = t.Consultorio?.NroConsultorio ?? 0,
            Pagado = t.Pago is not null
        };
    }
}
