using System.ComponentModel.DataAnnotations;

namespace MVCMiniProject.ViewModels.Admin
{
    public class IconUpdateVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad mütləqdir")]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
