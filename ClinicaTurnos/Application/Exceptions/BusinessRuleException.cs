namespace ClinicaTurnos.Application.Exceptions
{
    // Se lanza cuando no se cumple una regla del negocio → el handler global responde 400
    public class BusinessRuleException : Exception
    {
        public BusinessRuleException(string message) : base(message)
        {
        }
    }
}
