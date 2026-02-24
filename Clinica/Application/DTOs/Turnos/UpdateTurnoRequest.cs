using System.ComponentModel.DataAnnotations;

namespace TurnosClinica.Application.DTOs.Turnos
{
    public class UpdateTurnoRequest
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Debes seleccionar la fecha del turno.")]
        [DataType(DataType.Date)]
        public DateTime FechaTurno { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un paciente, es obligatoria.")]
        public Guid PacienteId { get; set; }
        public string PacienteNombreCompleto { get; set; } = "";
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un medico, es obligatoria.")]
        public int MedicoId { get; set; }
        public string MedicoNombreCompleto { get; set; } = "";

        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un estado, es obligatoria.")]
        public int EstadoId { get; set; }
        public string Estado { get; set; } = "";
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un consultorio, es obligatoria.")]
        public int ConsultorioId { get; set; }
        public string Consultorio { get; set; } = "";
        public bool TienePago { get; set; }
    }
}
