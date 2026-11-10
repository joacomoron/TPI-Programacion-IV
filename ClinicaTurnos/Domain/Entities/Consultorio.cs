namespace ClinicaTurnos.Domain.Entities
{
    public class Consultorio
    {
        public int Id { get; set; }
        public int NroConsultorio { get; set; }
        public bool Disponibilidad { get; set; } = true;

        // Relación 1 a N: un consultorio se asigna a muchos turnos
        public ICollection<Turno> Turnos { get; set; } = new List<Turno>();
    }
}
