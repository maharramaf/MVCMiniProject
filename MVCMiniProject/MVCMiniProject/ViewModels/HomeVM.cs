using MVCMiniProject.ViewModels.Events;
using MVCMiniProject.ViewModels.Icons;
using MVCMiniProject.ViewModels.Sliders;

namespace MVCMiniProject.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<IconUIVM> Icon { get; set; }
        public  IEnumerable<SliderUIVM> Slider  { get; set; }
        public  Dictionary<string , string > Settings { get; set; }
        public IEnumerable<EventUIVM> Events { get; set; }
    }
}
