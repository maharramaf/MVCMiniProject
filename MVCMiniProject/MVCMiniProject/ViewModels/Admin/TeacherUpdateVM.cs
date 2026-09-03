using System.ComponentModel.DataAnnotations;

namespace MVCMiniProject.ViewModels.Admin
{
    public class TeacherUpdateVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad Soyad mütləqdir")]
        [MaxLength(200)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Vəzifə seçilməlidir")]
        public int PositionId { get; set; }

        public IFormFile? Image { get; set; }

        public string? ExistingImage { get; set; }
    }
}
