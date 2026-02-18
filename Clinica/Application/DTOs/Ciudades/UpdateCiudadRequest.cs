using System.ComponentModel.DataAnnotations;

namespace TurnosClinica.Application.DTOs.Ciudades
{
    public class UpdateCiudadRequest
    {
        [Required(ErrorMessage = "Nombre es obligatorio.")]
        [MaxLength(120)]
        public string Nombre { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "La provincia es obligatoria.")]
        public int ProvinciaId { get; set; }
    }

}
