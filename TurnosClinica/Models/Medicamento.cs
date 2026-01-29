namespace TurnosClinica.Models
{
    public class Medicamento
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<RecetaMedicamento> Recetas { get; set; }
    }
}
