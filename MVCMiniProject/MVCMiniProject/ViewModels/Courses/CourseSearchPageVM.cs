namespace MVCMiniProject.ViewModels.Courses
{
    public class CourseSearchPageVM
    {
        public string Query { get; set; } = string.Empty;
        public IReadOnlyList<CourseSearchVM> Courses { get; set; } = Array.Empty<CourseSearchVM>();
    }
}
