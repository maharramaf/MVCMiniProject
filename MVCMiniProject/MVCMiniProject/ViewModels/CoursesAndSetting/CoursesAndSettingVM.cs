using MVCMiniProject.ViewModels.Courses;

namespace MVCMiniProject.ViewModels.CoursesAndSetting
{
    public class CoursesAndSettingVM
    {
        public Dictionary<string, string> Settings { get; set; }
        public IEnumerable<CourseInfoUIVM> CourseInfos { get; set; }

    }
}
