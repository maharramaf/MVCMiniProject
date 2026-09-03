using Microsoft.EntityFrameworkCore;
using MVCMiniProject.Data;
using MVCMiniProject.Models;
using MVCMiniProject.Services.Interfaces;
using MVCMiniProject.ViewModels.Admin;
using MVCMiniProject.ViewModels.Icons;
using MVCMiniProject.ViewModels.Sliders;

namespace MVCMiniProject.Services
{
    public class SliderService : ISliderService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderService(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IEnumerable<SliderUIVM>> GetAllUIAsync()
        {
            IEnumerable<SliderUIVM> sliders = await _context.Sliders.Select(m => new SliderUIVM
            {
                Logo = m.Logo,
                Description = m.Description,
                Image = m.Image,
                Name = m.Name
            }).ToListAsync();
            return sliders;
        }

        public async Task<IEnumerable<Slider>> GetAllAsync()
        {
            return await _context.Sliders.ToListAsync();
        }

        public async Task<Slider> GetByIdAsync(int id)
        {
            return await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task CreateAsync(SliderCreateVM sliderVM)
        {
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(sliderVM.Image.FileName);
            string path = Path.Combine(_env.WebRootPath, "images", fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await sliderVM.Image.CopyToAsync(stream);
            }

            Slider slider = new Slider
            {
                Name = sliderVM.Title,
                Description = sliderVM.Description,
                Image = fileName,
                Logo = fileName
            };

            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(SliderUpdateVM sliderVM)
        {
            var slider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == sliderVM.Id);
            if (slider == null) return;

            if (sliderVM.Image != null)
            {
                string oldImagePath = Path.Combine(_env.WebRootPath, "images", slider.Image);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(sliderVM.Image.FileName);
                string path = Path.Combine(_env.WebRootPath, "images", fileName);

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    await sliderVM.Image.CopyToAsync(stream);
                }

                slider.Image = fileName;
                slider.Logo = fileName;
            }

            slider.Name = sliderVM.Title;
            slider.Description = sliderVM.Description;

            _context.Sliders.Update(slider);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var slider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (slider == null) return;

            string imagePath = Path.Combine(_env.WebRootPath, "images", slider.Image);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
        }
    }
}
