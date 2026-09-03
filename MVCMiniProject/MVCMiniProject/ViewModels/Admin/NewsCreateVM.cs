using System.ComponentModel.DataAnnotations;

namespace MVCMiniProject.ViewModels.Admin
{
    public class NewsCreateVM
    {
        [Required(ErrorMessage = "Tarix mütləqdir")]
        [MaxLength(50)]
        public string Date { get; set; }

        [Required(ErrorMessage = "Açıqlama mütləqdir")]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Şəkil mütləqdir")]
        public IFormFile Image { get; set; }
    }
}
