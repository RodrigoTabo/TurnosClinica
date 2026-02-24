namespace TurnosClinica.Application.Services.Email
{
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string htmlBody, string? textBody = null);
    }
}

