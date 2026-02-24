using System.ComponentModel.DataAnnotations;

namespace TurnosClinica.Application.DTOs.Paises
{
    public class UpdatePaisRequest
    {
        [Required(ErrorMessage = "Nombre es obligatorio.")]
        [MaxLength(120)]
        public string Nombre { get; set; } = "";
    }
}
