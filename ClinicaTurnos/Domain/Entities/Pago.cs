namespace ClinicaTurnos.Domain.Entities
{
    public class Pago
    {
        public int Id { get; set; }
        public string MedioPago { get; set; } = string.Empty;
        public string Comprobante { get; set; } = string.Empty;

        // 1 a 1 con Turno: el pago guarda la FK del turno que abona
        public int TurnoId { get; set; }
        public Turno? Turno { get; set; }
    }
}
