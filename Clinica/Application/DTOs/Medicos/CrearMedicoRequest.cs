
using System.ComponentModel.DataAnnotations;

namespace TurnosClinica.Application.DTOs.Medicos
{
    public class CrearMedicoRequest
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatoria.")]
        [MaxLength(50)]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El Apellido es obligatoria.")]
        [MaxLength(30)]
        public string Apellido { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "El DNI es obligatoria.")]
        public int DNI { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "La especialidad es obligatoria.")]
        public int EspecialidadId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "El consultorio es obligatoria.")]
        public int ConsultorioId { get; set; }
    }
}
