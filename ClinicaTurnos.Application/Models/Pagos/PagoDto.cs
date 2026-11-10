using ClinicaTurnos.Domain.Entities;

namespace ClinicaTurnos.Application.Models.Pagos
{
    public class PagoDto
    {
        public int Id { get; set; }
        public int TurnoId { get; set; }
        public string MedioPago { get; set; } = string.Empty;
        public string Comprobante { get; set; } = string.Empty;

        public static PagoDto FromEntity(Pago p) => new()
        {
            Id = p.Id,
            TurnoId = p.TurnoId,
            MedioPago = p.MedioPago,
            Comprobante = p.Comprobante
        };
    }
}
