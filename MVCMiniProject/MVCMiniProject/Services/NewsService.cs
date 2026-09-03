using Microsoft.EntityFrameworkCore;
using MVCMiniProject.Data;
using MVCMiniProject.Models;
using MVCMiniProject.Services.Interfaces;
using MVCMiniProject.ViewModels.Admin;
using MVCMiniProject.ViewModels.News;

namespace MVCMiniProject.Services
{
    public class NewsService : INewsService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public NewsService(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
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

        public async Task<IEnumerable<News>> GetAllAsync()
        {
            return await _context.News.Include(n => n.Author).ToListAsync();
        }

        public async Task<News> GetByIdAsync(int id)
        {
            return await _context.News.Include(n => n.Author).FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task CreateAsync(NewsCreateVM newsVM)
        {
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(newsVM.Image.FileName);
            string path = Path.Combine(_env.WebRootPath, "images", fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await newsVM.Image.CopyToAsync(stream);
            }

            var firstAuthor = await _context.Authors.FirstOrDefaultAsync();

            News news = new News
            {
                Date = newsVM.Date,
                Description = newsVM.Description,
                Image = fileName,
                AuthorId = firstAuthor?.Id ?? 1
            };

            await _context.News.AddAsync(news);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(NewsUpdateVM newsVM)
        {
            var news = await _context.News.FirstOrDefaultAsync(n => n.Id == newsVM.Id);
            if (news == null) return;

            if (newsVM.Image != null)
            {
                string oldImagePath = Path.Combine(_env.WebRootPath, "images", news.Image);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(newsVM.Image.FileName);
                string path = Path.Combine(_env.WebRootPath, "images", fileName);

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    await newsVM.Image.CopyToAsync(stream);
                }

                news.Image = fileName;
            }

            news.Date = newsVM.Date;
            news.Description = newsVM.Description;

            _context.News.Update(news);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var news = await _context.News.FirstOrDefaultAsync(n => n.Id == id);
            if (news == null) return;

            string imagePath = Path.Combine(_env.WebRootPath, "images", news.Image);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            _context.News.Remove(news);
            await _context.SaveChangesAsync();
        }
    }
}
