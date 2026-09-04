namespace MVCMiniProject.ViewModels.Courses
{
    public class CourseSearchVM
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Price { get; set; }
        public string? Image { get; set; }
        public string Excerpt { get; set; } = string.Empty;
    }
}
