namespace TurnosClinica.Models
{
    public class Pago
    {
        public int Id { get; set; }
        public List<Turno> Turnos { get; set; }
        public int PagoMetodoId { get; set; }
        public PagoMetodo PagoMetodo { get; set; }

    }
}
