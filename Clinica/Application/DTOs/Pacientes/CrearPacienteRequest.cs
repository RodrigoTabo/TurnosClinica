using System.ComponentModel.DataAnnotations;

namespace TurnosClinica.Application.DTOs.Pacientes
{
    public class CrearPacienteRequest
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatoria.")]
        [MaxLength(50)]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El Apellido es obligatoria.")]
        [MaxLength(30)]
        public string Apellido { get; set; }
        [Required(ErrorMessage = "Complete este Campo")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "El DNI es obligatoria.")]
        public string DNI { get; set; }
        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo electrónico inválido")]
        public string EmailPrincipal { get; set; }

    }
}
