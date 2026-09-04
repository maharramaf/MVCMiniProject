using System.ComponentModel.DataAnnotations.Schema;

namespace MVCMiniProject.Models
{
    public class AppUser : BaseEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool IsEmailVerified { get; set; }
        public string? VerificationCode { get; set; }
        public DateTime? VerificationExpiresAt { get; set; }
        public DateTime? LastVerificationSentAt { get; set; }

        [NotMapped]
        public string? PendingVerificationToken { get; set; }
    }
}
