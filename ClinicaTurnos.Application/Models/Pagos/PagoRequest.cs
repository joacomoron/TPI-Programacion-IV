using System.ComponentModel.DataAnnotations;

namespace ClinicaTurnos.Application.Models.Pagos
{
    public class PagoRequest
    {
        [Range(1, int.MaxValue)]
        public int TurnoId { get; set; }

        [Required, StringLength(30)]
        public string MedioPago { get; set; } = string.Empty;   // ej: "Efectivo", "Tarjeta", "Transferencia"

        [Required, StringLength(100)]
        public string Comprobante { get; set; } = string.Empty;
    }
}
