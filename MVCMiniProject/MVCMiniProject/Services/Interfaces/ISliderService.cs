using MVCMiniProject.ViewModels.Icons;
using MVCMiniProject.ViewModels.Sliders;

namespace MVCMiniProject.Services.Interfaces
{
    public interface  ISliderService
    {
        Task<IEnumerable<SliderUIVM>> GetAllAsync();
    }
}
