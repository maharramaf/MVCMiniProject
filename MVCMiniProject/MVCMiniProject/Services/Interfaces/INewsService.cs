using MVCMiniProject.Models;
using MVCMiniProject.ViewModels.Icons;
using MVCMiniProject.ViewModels.News;

namespace MVCMiniProject.Services.Interfaces
{
    public interface INewsService 
    {
        Task<IEnumerable<NewsUIVM>> GetAllUIAsync();
    }
}
