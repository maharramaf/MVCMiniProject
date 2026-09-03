using System.ComponentModel.DataAnnotations;

namespace MVCMiniProject.ViewModels.Admin
{
    public class SliderCreateVM
    {
        [Required(ErrorMessage = "Başlıq mütləqdir")]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Açıqlama mütləqdir")]
        [MaxLength(500)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Şəkil mütləqdir")]
        public IFormFile Image { get; set; }
    }
}
