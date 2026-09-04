namespace MVCMiniProject.Options
{
    public class SmtpOptions
    {
        public const string SectionName = "Smtp";

        public string Host { get; set; } = string.Empty;
        public int Port { get; set; } = 587;
        public string User { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;
        public string FromName { get; set; } = "Elearn";
        public bool EnableSsl { get; set; } = true;
        public int VerificationHours { get; set; } = 24;
        public int ResendCooldownSeconds { get; set; } = 90;
    }
}
