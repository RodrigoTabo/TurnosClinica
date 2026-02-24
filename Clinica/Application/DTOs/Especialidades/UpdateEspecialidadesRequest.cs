using System.ComponentModel.DataAnnotations;

namespace TurnosClinica.Application.DTOs.Especialidades
{
    public class UpdateEspecialidadesRequest
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatoria.")]
        [MaxLength(120)]
        public string Nombre { get; set; }
    }
}
