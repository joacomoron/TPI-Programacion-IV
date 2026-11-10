using ClinicaTurnos.Domain.Entities;

namespace ClinicaTurnos.Application.Models.Consultorios
{
    public class ConsultorioDto
    {
        public int Id { get; set; }
        public int NroConsultorio { get; set; }
        public bool Disponibilidad { get; set; }

        public static ConsultorioDto FromEntity(Consultorio c) => new()
        {
            Id = c.Id,
            NroConsultorio = c.NroConsultorio,
            Disponibilidad = c.Disponibilidad
        };
    }
}
