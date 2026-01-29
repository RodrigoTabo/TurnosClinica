namespace TurnosClinica.Models
{
    public class Consultorio
    {

        public int Id { get; set; }
        public string Institucion { get; set; }
        public int Calle { get; set; }
        public int Altura { get; set; }
        public int CiudadId { get; set; }
        public Ciudad Ciudad { get; set; }
        public List<Turno> Turnos { get; set; }

    }
}
