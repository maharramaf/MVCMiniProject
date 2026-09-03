using MVCMiniProject.Models;
using MVCMiniProject.ViewModels.Admin;
using MVCMiniProject.ViewModels.Icons;
using MVCMiniProject.ViewModels.News;

namespace MVCMiniProject.Services.Interfaces
{
    public interface INewsService 
    {
        Task<IEnumerable<NewsUIVM>> GetAllUIAsync();
        Task<IEnumerable<News>> GetAllAsync();
        Task<News> GetByIdAsync(int id);
        Task CreateAsync(NewsCreateVM newsVM);
        Task UpdateAsync(NewsUpdateVM newsVM);
        Task DeleteAsync(int id);
    }
}
