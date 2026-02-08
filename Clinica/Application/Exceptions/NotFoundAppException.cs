namespace TurnosClinica.Application.Exceptions
{
    public class NotFoundAppException : Exception
    {
        public NotFoundAppException(string message) : base(message) { }
    }
}
