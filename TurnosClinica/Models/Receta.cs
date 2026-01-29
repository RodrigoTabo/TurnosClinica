namespace TurnosClinica.Models
{
    public class Receta
    {

        public int Id { get; set; }
        public int HistoriaClinicaId { get; set; }
        public HistoriaClinica HistoriaClinica { get; set; }
        public List<RecetaMedicamento> Items { get; set; }
        public DateTime Fecha { get; set; }
        public string? Observaciones { get; set; }

    }
}
