namespace TurnosClinica.Models
{
    public class PagoMetodo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<Pago> Pagos { get; set; } = new();


    }
}
