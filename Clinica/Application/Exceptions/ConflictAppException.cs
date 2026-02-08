namespace TurnosClinica.Application.Exceptions
{
    public class ConflictAppException : Exception
    {
        public ConflictAppException(string message) : base(message) { }
    }
}
