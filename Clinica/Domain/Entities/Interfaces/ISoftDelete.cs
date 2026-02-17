namespace TurnosClinica.Domain.Entities.Interfaces
{
    public interface ISoftDelete
    {
        DateTime? EliminadoEn { get; set; }
    }
}
