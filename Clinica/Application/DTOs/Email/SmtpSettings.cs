namespace TurnosClinica.Application.DTOs.Email
{
    public sealed class SmtpSettings
    {
        public string Host { get; set; } = "";
        public int Port { get; set; }
        public string User { get; set; } = "";
        public string Password { get; set; } = "";
        public string FromName { get; set; } = "Sistema";
        public string FromEmail { get; set; } = ""; 
        public bool UseStartTls { get; set; } = true;
    }
}
