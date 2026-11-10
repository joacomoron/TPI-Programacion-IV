namespace ClinicaTurnos.Domain.Interfaces
{
    // Contrato para consultar feriados. La implementación (Infrastructure) consume una API externa,
    // pero el resto de la app no sabe ni le importa de dónde salen los datos.
    public interface IFeriadoService
    {
        Task<bool> EsFeriadoAsync(DateOnly fecha);
    }
}
