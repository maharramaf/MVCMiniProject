using System.ComponentModel.DataAnnotations;

namespace MVCMiniProject.ViewModels.Admin
{
    public class CourseCreateVM
    {
        [Required(ErrorMessage = "Başlıq mütləqdir")]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Açıqlama mütləqdir")]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Qiymət mütləqdir")]
        [Range(0, 10000)]
        public decimal Price { get; set; }

        public int SalesCount { get; set; } = 0;

        public bool IsFeature { get; set; } = false;

        public bool IsNew { get; set; } = false;

        [Required(ErrorMessage = "Müəllim seçilməlidir")]
        public int TeacherId { get; set; }

        [Required(ErrorMessage = "Şəkil mütləqdir")]
        public IFormFile MainImage { get; set; }
    }
}
