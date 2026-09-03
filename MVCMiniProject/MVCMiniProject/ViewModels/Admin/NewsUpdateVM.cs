using System.ComponentModel.DataAnnotations;

namespace MVCMiniProject.ViewModels.Admin
{
    public class NewsUpdateVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tarix mütləqdir")]
        [MaxLength(50)]
        public string Date { get; set; }

        [Required(ErrorMessage = "Açıqlama mütləqdir")]
        [MaxLength(1000)]
        public string Description { get; set; }

        public IFormFile? Image { get; set; }

        public string? ExistingImage { get; set; }
    }
}
