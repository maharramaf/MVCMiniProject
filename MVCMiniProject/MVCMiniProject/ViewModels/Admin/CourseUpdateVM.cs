using System.ComponentModel.DataAnnotations;

namespace MVCMiniProject.ViewModels.Admin
{
    public class CourseUpdateVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [MaxLength(4000)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0, 100000, ErrorMessage = "Price must be between 0 and 100000.")]
        public int Price { get; set; }

        [Range(0, 1000000, ErrorMessage = "Sales count cannot be negative.")]
        public int SalesCount { get; set; }

        public bool IsFeature { get; set; }

        public bool IsNew { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Teacher is required.")]
        public int TeacherId { get; set; }

        public IFormFile? MainImage { get; set; }

        public string? ExistingImage { get; set; }
    }
}
