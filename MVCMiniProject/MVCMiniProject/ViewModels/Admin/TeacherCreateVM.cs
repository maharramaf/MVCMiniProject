using System.ComponentModel.DataAnnotations;

namespace MVCMiniProject.ViewModels.Admin
{
    public class TeacherCreateVM
    {
        [Required(ErrorMessage = "Ad Soyad mütləqdir")]
        [MaxLength(200)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Vəzifə seçilməlidir")]
        public int PositionId { get; set; }

        [Required(ErrorMessage = "Şəkil mütləqdir")]
        public IFormFile Image { get; set; }
    }
}
