using Microsoft.Extensions.Options;
using MVCMiniProject.Helpers;
using MVCMiniProject.Models;
using MVCMiniProject.Options;
using MVCMiniProject.Services.Interfaces;

namespace MVCMiniProject.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IEmailSender _emailSender;
        private readonly SmtpOptions _smtpOptions;

        public AuthService(IUserService userService, IEmailSender emailSender, IOptions<SmtpOptions> smtpOptions)
        {
            _userService = userService;
            _emailSender = emailSender;
            _smtpOptions = smtpOptions.Value;
        }

        public async Task<(bool Ok, string Message)> RegisterAsync(string firstName, string lastName, string email, string password, string verifyBaseUrl)
        {
            email = email.Trim().ToLowerInvariant();
            if (await _userService.EmailExistsAsync(email))
            {
                return (false, "This email is already registered.");
            }

            var user = new AppUser
            {
                Username = email,
                Email = email,
                FullName = $"{firstName.Trim()} {lastName.Trim()}".Trim(),
                Password = PasswordHelper.Hash(password),
                Role = "user",
                IsEmailVerified = false
            };

            AssignVerification(user);
            await _userService.CreateAsync(user);

            try
            {
                await SendVerificationAsync(user, verifyBaseUrl);
            }
            catch
            {
                return (true, "Account created, but the confirmation email could not be sent. Use Resend verification after checking SMTP settings.");
            }

            return (true, "Account created. Please confirm your email to sign in.");
        }

        public async Task<(bool Ok, string Message, AppUser? User, bool NeedsVerification)> ValidateLoginAsync(string email, string password)
        {
            var user = await _userService.GetByEmailAsync(email.Trim().ToLowerInvariant());
            if (user == null || !PasswordHelper.Verify(password, user.Password))
            {
                return (false, "Invalid email or password.", null, false);
            }

            if (!user.IsEmailVerified)
            {
                return (false, "You need to verify your email address.", user, true);
            }

            return (true, "OK", user, false);
        }

        public async Task<(bool Ok, string Message)> ConfirmEmailAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return (false, "This confirmation link is invalid.");
            }

            string hash;
            try
            {
                hash = VerificationTokenHelper.Hash(token);
            }
            catch
            {
                return (false, "This confirmation link is invalid.");
            }

            var user = await _userService.GetByVerificationHashAsync(hash);
            if (user == null)
            {
                return (false, "This confirmation link is invalid or has already been used.");
            }

            if (user.IsEmailVerified)
            {
                user.VerificationCode = null;
                user.VerificationExpiresAt = null;
                await _userService.UpdateAsync(user);
                return (true, "Your email is already verified. You can sign in.");
            }

            if (user.VerificationExpiresAt == null || user.VerificationExpiresAt < DateTime.UtcNow)
            {
                return (false, "This confirmation link has expired. Please request a new email.");
            }

            user.IsEmailVerified = true;
            user.VerificationCode = null;
            user.VerificationExpiresAt = null;
            await _userService.UpdateAsync(user);
            return (true, "Your email has been confirmed. You can now sign in.");
        }

        public async Task<(bool Ok, string Message)> ResendVerificationAsync(string email, string verifyBaseUrl)
        {
            var user = await _userService.GetByEmailAsync(email.Trim().ToLowerInvariant());
            if (user == null)
            {
                return (true, "If this email is registered, a new confirmation message has been sent.");
            }

            if (user.IsEmailVerified)
            {
                return (true, "This email is already verified. You can sign in.");
            }

            var cooldown = Math.Max(30, _smtpOptions.ResendCooldownSeconds);
            if (user.LastVerificationSentAt.HasValue &&
                user.LastVerificationSentAt.Value.AddSeconds(cooldown) > DateTime.UtcNow)
            {
                var wait = (int)Math.Ceiling((user.LastVerificationSentAt.Value.AddSeconds(cooldown) - DateTime.UtcNow).TotalSeconds);
                return (false, $"Please wait {wait} seconds before requesting another confirmation email.");
            }

            AssignVerification(user);
            await _userService.UpdateAsync(user);

            try
            {
                await SendVerificationAsync(user, verifyBaseUrl);
            }
            catch
            {
                return (false, "The confirmation email could not be sent. Check SMTP settings.");
            }

            return (true, "If this email is registered, a new confirmation message has been sent.");
        }

        private void AssignVerification(AppUser user)
        {
            var (plain, hash) = VerificationTokenHelper.Create();
            user.VerificationCode = hash;
            user.VerificationExpiresAt = DateTime.UtcNow.AddHours(Math.Max(1, _smtpOptions.VerificationHours));
            user.LastVerificationSentAt = DateTime.UtcNow;
            user.PendingVerificationToken = plain;
        }

        private async Task SendVerificationAsync(AppUser user, string verifyBaseUrl)
        {
            var token = user.PendingVerificationToken;
            if (string.IsNullOrWhiteSpace(token))
            {
                return;
            }

            var link = $"{verifyBaseUrl.TrimEnd('/')}/auth/verify-email?token={Uri.EscapeDataString(token)}";
            var html = EmailTemplates.Verification(
                string.IsNullOrWhiteSpace(_smtpOptions.FromName) ? "Elearn" : _smtpOptions.FromName,
                user.FullName,
                link,
                Math.Max(1, _smtpOptions.VerificationHours));

            await _emailSender.SendAsync(user.Email, "Confirm your Elearn email", html);
            user.PendingVerificationToken = null;
        }
    }
}
