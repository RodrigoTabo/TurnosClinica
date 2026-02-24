using System.ComponentModel.DataAnnotations;

namespace TurnosClinica.Application.DTOs.Consultorios
{
    public class UpdateConsultorioRequest
    {
        [Required(ErrorMessage = "El nombre de la institución es requerida.")]
        [MaxLength(120)]
        public string Institucion { get; set; }
        [Required(ErrorMessage = "La calle de la institución es requerida.")]
        [MaxLength(120)]
        public string Calle { get; set; } = "";
        [Required(ErrorMessage = "El número de la institución es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "El número de la institución debe ser mayor a 0.")]
        public int Altura { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "La ciudad de la institución es requerido.")]
        public int CiudadId { get; set; }
    }
}
