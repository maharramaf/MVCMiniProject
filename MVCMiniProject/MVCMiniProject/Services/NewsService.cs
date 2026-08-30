using MVCMiniProject.Data;
using MVCMiniProject.Services.Interfaces;
using MVCMiniProject.ViewModels.News;

namespace MVCMiniProject.Services
{
    public class NewsService : INewsService
    {

        private readonly AppDbContext _context;
        public NewsService(AppDbContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<NewsUIVM>> GetAllUIAsync()
        {
            throw new NotImplementedException();
        }
    }
}
