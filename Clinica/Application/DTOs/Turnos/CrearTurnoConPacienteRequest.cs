using System.ComponentModel.DataAnnotations;

namespace TurnosClinica.Application.DTOs.Turnos
{
    public class CrearTurnoConPacienteRequest
    {
        [Required(ErrorMessage = "El nombre es obligatoria.")]
        [MaxLength(50)]
        public string Nombre { get; set; } = default!;
        [Required(ErrorMessage = "El Apellido es obligatoria.")]
        [MaxLength(20)]
        public string Apellido { get; set; } = default!;
        [Required(ErrorMessage = "Complete este Campo")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; } = default!;
        [Range(1, int.MaxValue, ErrorMessage = "El DNI es obligatoria.")]
        public string DNI { get; set; } = default!;
        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo electrónico inválido")]
        public string EmailPrincipal { get; set; } = default!;

        // Datos del turno
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un medico, es obligatoria.")]
        public int MedicoId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un consultorio, es obligatoria.")]
        public int ConsultorioId { get; set; }
        [Required(ErrorMessage = "Debes seleccionar la fecha del turno.")]
        [DataType(DataType.Date)]
        public DateTime FechaTurno { get; set; } 
        public int? EstadoId { get; set; }
    }
}
