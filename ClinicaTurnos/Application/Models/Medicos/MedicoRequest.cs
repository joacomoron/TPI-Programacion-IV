using System.ComponentModel.DataAnnotations;

namespace ClinicaTurnos.Application.Models.Medicos
{
    public class MedicoRequest
    {
        [Required, StringLength(50)]
        public string Nombre { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Apellido { get; set; } = string.Empty;

        [Required, RegularExpression(@"^\d{7,8}$", ErrorMessage = "El DNI debe tener 7 u 8 números")]
        public string Dni { get; set; } = string.Empty;

        [Required, StringLength(20)]
        public string Matricula { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Especialidad { get; set; } = string.Empty;
    }
}
