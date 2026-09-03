using System.ComponentModel.DataAnnotations;

namespace MVCMiniProject.ViewModels.Admin
{
    public class SettingUpdateVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Key mütləqdir")]
        [MaxLength(100)]
        public string Key { get; set; }

        [Required(ErrorMessage = "Value mütləqdir")]
        [MaxLength(500)]
        public string Value { get; set; }
    }
}
