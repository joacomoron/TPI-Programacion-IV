namespace ClinicaTurnos.Application.Exceptions
{
    // Se lanza cuando se busca algo que no existe → el handler global responde 404
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {
        }
    }
}
