using MVCMiniProject.Models;
using MVCMiniProject.ViewModels.Admin;
using MVCMiniProject.ViewModels.Icons;
using MVCMiniProject.ViewModels.Sliders;

namespace MVCMiniProject.Services.Interfaces
{
    public interface ISliderService
    {
        Task<IEnumerable<SliderUIVM>> GetAllUIAsync();
        Task<IEnumerable<Slider>> GetAllAsync();
        Task<Slider> GetByIdAsync(int id);
        Task CreateAsync(SliderCreateVM sliderVM);
        Task UpdateAsync(SliderUpdateVM sliderVM);
        Task DeleteAsync(int id);
    }
}
