namespace TurnosClinica.Application.Services.Email
{
    using MailKit.Net.Smtp;
    using MailKit.Security;
    using Microsoft.Extensions.Options;
    using MimeKit;
    using TurnosClinica.Application.DTOs.Email;

    public sealed class SmtpEmailService : IEmailService
    {
        private readonly SmtpSettings _cfg;

        public SmtpEmailService(IOptions<SmtpSettings> cfg)
            => _cfg = cfg.Value;

        public async Task SendAsync(string to, string subject, string htmlBody, string? textBody = null)
        {
            if (string.IsNullOrWhiteSpace(_cfg.FromEmail))
                throw new InvalidOperationException("SmtpSettings:FromEmail no está configurado.");

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_cfg.FromName, _cfg.FromEmail));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;

            var builder = new BodyBuilder
            {
                HtmlBody = htmlBody,
                TextBody = textBody ?? string.Empty
            };

            message.Body = builder.ToMessageBody();

            using var client = new SmtpClient();

            await client.ConnectAsync(
                _cfg.Host,
                _cfg.Port,
                _cfg.UseStartTls ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto
            );

            await client.AuthenticateAsync(_cfg.User, _cfg.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
