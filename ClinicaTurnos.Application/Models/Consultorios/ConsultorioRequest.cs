using System.ComponentModel.DataAnnotations;

namespace ClinicaTurnos.Application.Models.Consultorios
{
    public class ConsultorioRequest
    {
        [Range(1, 999, ErrorMessage = "El número de consultorio debe estar entre 1 y 999")]
        public int NroConsultorio { get; set; }

        public bool Disponibilidad { get; set; } = true;
    }
}
