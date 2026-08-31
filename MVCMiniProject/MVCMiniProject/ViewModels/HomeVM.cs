using MVCMiniProject.ViewModels.Courses;
using MVCMiniProject.ViewModels.Events;
using MVCMiniProject.ViewModels.Icons;
using MVCMiniProject.ViewModels.News;
using MVCMiniProject.ViewModels.Sliders;
using MVCMiniProject.ViewModels.Videos;

namespace MVCMiniProject.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<IconUIVM> Icon { get; set; }
        public  IEnumerable<SliderUIVM> Slider  { get; set; }
        public  Dictionary<string , string > Settings { get; set; }
        public IEnumerable<EventUIVM> Events { get; set; }

        public IEnumerable<NewsUIVM> News { get; set; }
        public VideoUIVM  Video{ get; set; }
        public IEnumerable<CourseInfoUIVM> CourseInfos { get; set; }
    }
}
