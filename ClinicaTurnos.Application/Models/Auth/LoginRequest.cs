using System.ComponentModel.DataAnnotations;

namespace ClinicaTurnos.Application.Models.Auth
{
    public class LoginRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Dni { get; set; } = string.Empty;
    }
}
