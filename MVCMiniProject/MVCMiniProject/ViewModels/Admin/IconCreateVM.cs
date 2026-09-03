using System.ComponentModel.DataAnnotations;

namespace MVCMiniProject.ViewModels.Admin
{
    public class IconCreateVM
    {
        [Required(ErrorMessage = "Ad mütləqdir")]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
