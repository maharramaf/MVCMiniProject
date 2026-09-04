using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MVCMiniProject.Options;
using MVCMiniProject.Services.Interfaces;

namespace MVCMiniProject.Services
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly SmtpOptions _options;
        private readonly ILogger<SmtpEmailSender> _logger;

        public SmtpEmailSender(IOptions<SmtpOptions> options, ILogger<SmtpEmailSender> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        public bool IsConfigured =>
            !string.IsNullOrWhiteSpace(_options.Host) &&
            !string.IsNullOrWhiteSpace(_options.From);

        public async Task SendAsync(string toEmail, string subject, string htmlBody)
        {
            if (!IsConfigured)
            {
                throw new InvalidOperationException("SMTP is not configured. Set SMTP_HOST and SMTP_FROM in the .env file.");
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(
                string.IsNullOrWhiteSpace(_options.FromName) ? "Elearn" : _options.FromName,
                _options.From));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;
            message.Body = new BodyBuilder { HtmlBody = htmlBody }.ToMessageBody();

            using var client = new SmtpClient();
            var secure = _options.EnableSsl
                ? (_options.Port == 465 ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls)
                : SecureSocketOptions.None;

            await client.ConnectAsync(_options.Host, _options.Port <= 0 ? 587 : _options.Port, secure);
            if (!string.IsNullOrWhiteSpace(_options.User))
            {
                await client.AuthenticateAsync(_options.User, _options.Password);
            }

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
            _logger.LogInformation("Verification email queued to {Email}", toEmail);
        }
    }
}
