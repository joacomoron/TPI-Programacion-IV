using ClinicaTurnos.Domain.Entities;

namespace ClinicaTurnos.Application.Models.Pacientes
{
    public class PacienteDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Dni { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string ObraSocial { get; set; } = string.Empty;

        public static PacienteDto FromEntity(Paciente p) => new()
        {
            Id = p.Id,
            Nombre = p.Nombre,
            Apellido = p.Apellido,
            Dni = p.Dni,
            Email = p.Email,
            Direccion = p.Direccion,
            ObraSocial = p.ObraSocial
        };
    }
}
