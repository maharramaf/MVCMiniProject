using MVCMiniProject.ViewModels.Courses;

namespace MVCMiniProject.ViewModels.SettingAndCourse
{
    public class SettingAndCourseVM
    {
        public Dictionary<string, string> Settings { get; set; }
        public IEnumerable<CourseInfoUIVM> CourseInfos { get; set; }
    }
}
