namespace ClinicaTurnos.Application.Models.Auth
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public int PacienteId { get; set; }
        public string Paciente { get; set; } = string.Empty;
    }
}
