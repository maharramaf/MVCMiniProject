using MVCMiniProject.Models;

namespace MVCMiniProject.ViewModels.Admin
{
    public class CourseAdminListVM
    {
        public IEnumerable<CourseInfo> Courses { get; set; } = Enumerable.Empty<CourseInfo>();
        public string? Search { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }
        public int TotalPages => PageSize <= 0 ? 1 : (int)Math.Ceiling(TotalCount / (double)PageSize);
    }
}
