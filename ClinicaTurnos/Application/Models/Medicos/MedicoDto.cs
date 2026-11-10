using ClinicaTurnos.Domain.Entities;

namespace ClinicaTurnos.Application.Models.Medicos
{
    public class MedicoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Dni { get; set; } = string.Empty;
        public string Matricula { get; set; } = string.Empty;
        public string Especialidad { get; set; } = string.Empty;

        public static MedicoDto FromEntity(Medico m) => new()
        {
            Id = m.Id,
            Nombre = m.Nombre,
            Apellido = m.Apellido,
            Dni = m.Dni,
            Matricula = m.Matricula,
            Especialidad = m.Especialidad
        };
    }
}
