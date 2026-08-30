using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<NewsUIVM>> GetAllUIAsync()
        {
            IEnumerable<NewsUIVM> news = await _context.News.Include(m => m.Author).Select(a => new NewsUIVM
            {
                Date = a.Date,
                Description = a.Description,
                Image = a.Image,
                AuthorName = a.Author.FullName
            }).ToListAsync();
              return news;
        }
    }
}
