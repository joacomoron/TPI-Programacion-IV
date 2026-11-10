namespace ClinicaTurnos.Application.Exceptions
{
    // Credenciales inválidas → el handler global responde 401
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message) : base(message)
        {
        }
    }
}
