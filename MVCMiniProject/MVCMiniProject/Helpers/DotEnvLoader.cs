using System.Globalization;
using System.Linq;

namespace MVCMiniProject.Helpers
{
    public static class DotEnvLoader
    {
        public static void Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            foreach (var rawLine in File.ReadAllLines(filePath))
            {
                var line = rawLine.Trim();
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#", StringComparison.Ordinal))
                {
                    continue;
                }

                var separator = line.IndexOf('=');
                if (separator <= 0)
                {
                    continue;
                }

                var key = line[..separator].Trim();
                var value = line[(separator + 1)..].Trim().Trim('"').Trim('\'');
                if (!string.IsNullOrWhiteSpace(key))
                {
                    Environment.SetEnvironmentVariable(key, value);
                }
            }
        }

        public static Dictionary<string, string?> ToSmtpConfiguration()
        {
            var pairs = new Dictionary<string, string?>
            {
                ["Smtp:Host"] = Environment.GetEnvironmentVariable("SMTP_HOST"),
                ["Smtp:Port"] = Environment.GetEnvironmentVariable("SMTP_PORT"),
                ["Smtp:User"] = Environment.GetEnvironmentVariable("SMTP_USER"),
                ["Smtp:Password"] = Environment.GetEnvironmentVariable("SMTP_PASSWORD"),
                ["Smtp:From"] = Environment.GetEnvironmentVariable("SMTP_FROM"),
                ["Smtp:FromName"] = Environment.GetEnvironmentVariable("SMTP_FROM_NAME"),
                ["Smtp:EnableSsl"] = Environment.GetEnvironmentVariable("SMTP_ENABLE_SSL")
            };

            return pairs
                .Where(p => p.Value != null)
                .ToDictionary(p => p.Key, p => p.Value);
        }

        public static int ReadPort(IConfiguration configuration)
        {
            var raw = configuration["Smtp:Port"];
            return int.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out var port) ? port : 587;
        }
    }
}
