namespace TurnosClinica.Models
{
    public class Pago
    {
        public int Id { get; set; }

        public int TurnoId { get; set; }
        public Turno Turno { get; set; } = null!;

        public int PagoMetodoId { get; set; }
        public PagoMetodo PagoMetodo { get; set; } = null!;
    }

}
