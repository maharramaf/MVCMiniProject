using MVCMiniProject.ViewModels.Icons;
using MVCMiniProject.ViewModels.Sliders;

namespace MVCMiniProject.ViewModels.Home
{
    public class HomeVM
    {
        public IEnumerable<IconUIVM> Icon { get; set; }
        public  IEnumerable<SliderUIVM> Slider  { get; set; }
    }
}
