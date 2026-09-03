using System.ComponentModel.DataAnnotations;

namespace MVCMiniProject.ViewModels.Admin
{
    public class CourseUpdateVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlıq mütləqdir")]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Açıqlama mütləqdir")]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Qiymət mütləqdir")]
        [Range(0, 10000)]
        public decimal Price { get; set; }

        public int SalesCount { get; set; }

        public bool IsFeature { get; set; }

        public bool IsNew { get; set; }

        [Required(ErrorMessage = "Müəllim seçilməlidir")]
        public int TeacherId { get; set; }

        public IFormFile? MainImage { get; set; }

        public string? ExistingImage { get; set; }
    }
}
