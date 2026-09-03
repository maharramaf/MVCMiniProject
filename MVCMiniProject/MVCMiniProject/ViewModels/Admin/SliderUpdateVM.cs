using System.ComponentModel.DataAnnotations;

namespace MVCMiniProject.ViewModels.Admin
{
    public class SliderUpdateVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlıq mütləqdir")]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Açıqlama mütləqdir")]
        [MaxLength(500)]
        public string Description { get; set; }

        public IFormFile? Image { get; set; }

        public string? ExistingImage { get; set; }
    }
}
