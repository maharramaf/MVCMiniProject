using System.ComponentModel.DataAnnotations;

namespace MVCMiniProject.ViewModels.Admin
{
    public class EventUpdateVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlıq mütləqdir")]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Açıqlama mütləqdir")]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Tarix mütləqdir")]
        [Range(1, 31)]
        public int Date { get; set; }

        [Required(ErrorMessage = "Ay mütləqdir")]
        [MaxLength(50)]
        public string Month { get; set; }
    }
}
