using Microsoft.AspNetCore.Mvc;
using MVCMiniProject.Data;
using MVCMiniProject.Models;

namespace MVCMiniProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PositionController : Controller
    {
        private readonly AppDbContext _context;

        public PositionController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAjax([FromBody] PositionCreateModel model)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                return Json(new { success = false });
            }

            var position = new Position
            {
                Name = model.Name
            };

            await _context.Positions.AddAsync(position);
            await _context.SaveChangesAsync();

            return Json(new { success = true, id = position.Id, name = position.Name });
        }
    }

    public class PositionCreateModel
    {
        public string Name { get; set; }
    }
}
