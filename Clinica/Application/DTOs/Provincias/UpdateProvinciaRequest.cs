using System.ComponentModel.DataAnnotations;

namespace TurnosClinica.Application.DTOs.Provincias
{
    public class UpdateProvinciaRequest
    {
        [Required(ErrorMessage = "Nombre es obligatorio.")]
        [MaxLength(120)]
        public string Nombre { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "El Pais es obligatoria.")]
        public int PaisId { get; set; }
        public DateTime? Eliminadoen { get; set; }
    }
}
