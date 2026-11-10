using ClinicaTurnos.Domain.Enums;

namespace ClinicaTurnos.Domain.Entities
{
    public class Turno
    {
        public int Id { get; set; }
        public DateOnly Fecha { get; set; }
        public TimeOnly Hora { get; set; }
        public EstadoTurno Estado { get; set; } = EstadoTurno.Pendiente;

        // N a 1 con Paciente
        public int PacienteId { get; set; }
        public Paciente? Paciente { get; set; }

        // N a 1 con Medico
        public int MedicoId { get; set; }
        public Medico? Medico { get; set; }

        // N a 1 con Consultorio
        public int ConsultorioId { get; set; }
        public Consultorio? Consultorio { get; set; }

        // 1 a 1 con Pago (puede no existir todavía si el turno no se pagó)
        public Pago? Pago { get; set; }
    }
}
