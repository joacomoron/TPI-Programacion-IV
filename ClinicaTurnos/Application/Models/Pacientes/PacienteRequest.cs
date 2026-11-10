using System.ComponentModel.DataAnnotations;

namespace ClinicaTurnos.Application.Models.Pacientes
{
    public class PacienteRequest
    {
        [Required, StringLength(50)]
        public string Nombre { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Apellido { get; set; } = string.Empty;

        [Required, RegularExpression(@"^\d{7,8}$", ErrorMessage = "El DNI debe tener 7 u 8 números")]
        public string Dni { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [StringLength(100)]
        public string Direccion { get; set; } = string.Empty;

        [StringLength(50)]
        public string ObraSocial { get; set; } = string.Empty;
    }
}
