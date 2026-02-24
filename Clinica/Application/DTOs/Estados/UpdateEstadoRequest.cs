using System.ComponentModel.DataAnnotations;

namespace TurnosClinica.Application.DTOs.Estados
{
    public class UpdateEstadoRequest
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatoria.")]
        [MaxLength(120)]
        public string Nombre { get; set; }
        public DateTime? EliminadoEn { get; set; }
    }
}
