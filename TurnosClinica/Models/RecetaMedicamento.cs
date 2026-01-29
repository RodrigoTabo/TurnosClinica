namespace TurnosClinica.Models
{
    public class RecetaMedicamento
    {
        public int Id { get; set; }
        public int RecetaId { get; set; }
        public Receta Receta { get; set; }
        public int MedicamentoId { get; set; }
        public Medicamento Medicamento { get; set; }
        public int Dosis { get; set; }
        public string Frecuencias { get; set; }
        public int Dias { get; set; }

    }
}
