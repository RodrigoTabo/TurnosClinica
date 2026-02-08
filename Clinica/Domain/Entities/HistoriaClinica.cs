namespace TurnosClinica.Models
{
    public class HistoriaClinica
    {
        public int Id { get; set; }
        public Guid PacienteId { get; set; }
        public Paciente Paciente { get; set; }
        public List<Receta> Recetas { get; set; } = new();
        public string Observaciones { get; set; }
        public string Alergias { get; set; }
    }
}
