namespace MVCMiniProject.Models
{
    public class News : BaseEntity
    {
        public  string  Date { get; set; }
        public string  Description { get; set; }
        public string  Image { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }

    }
}
