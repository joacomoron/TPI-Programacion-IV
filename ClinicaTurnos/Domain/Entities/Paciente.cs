namespace ClinicaTurnos.Domain.Entities
{
    public class Paciente
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Dni { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string ObraSocial { get; set; } = string.Empty;

        // Relación 1 a N: un paciente tiene muchos turnos
        public ICollection<Turno> Turnos { get; set; } = new List<Turno>();
    }
}
