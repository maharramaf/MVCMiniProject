using MVCMiniProject.Models;

namespace MVCMiniProject.Services.Interfaces
{
    public interface IUserService
    {
        Task<AppUser?> GetByUsernameAsync(string username);
        Task<AppUser?> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
        Task<AppUser> CreateAsync(AppUser user);
        Task UpdateAsync(AppUser user);
        Task<AppUser?> GetByVerificationHashAsync(string hash);
        Task<int> GetCountAsync();
        Task<IEnumerable<AppUser>> GetAllAsync();
    }

    public interface IEmailSender
    {
        bool IsConfigured { get; }
        Task SendAsync(string toEmail, string subject, string htmlBody);
    }

    public interface IAuthService
    {
        Task<(bool Ok, string Message)> RegisterAsync(string firstName, string lastName, string email, string password, string verifyBaseUrl);
        Task<(bool Ok, string Message, AppUser? User, bool NeedsVerification)> ValidateLoginAsync(string email, string password);
        Task<(bool Ok, string Message)> ConfirmEmailAsync(string token);
        Task<(bool Ok, string Message)> ResendVerificationAsync(string email, string verifyBaseUrl);
    }
}
