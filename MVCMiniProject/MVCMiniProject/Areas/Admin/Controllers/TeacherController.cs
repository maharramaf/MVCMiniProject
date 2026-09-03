using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVCMiniProject.Services.Interfaces;
using MVCMiniProject.ViewModels.Admin;

namespace MVCMiniProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeacherController : Controller
    {
        private readonly ITeacherService _teacherService;
        private readonly IPositionService _positionService;
        private readonly IWebHostEnvironment _env;

        public TeacherController(ITeacherService teacherService, IPositionService positionService, IWebHostEnvironment env)
        {
            _teacherService = teacherService;
            _positionService = positionService;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var teachers = await _teacherService.GetAllAsync();
            return View(teachers);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var teacher = await _teacherService.GetByIdAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Positions = new SelectList(await _positionService.GetAllAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeacherCreateVM teacherVM)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Positions = new SelectList(await _positionService.GetAllAsync(), "Id", "Name");
                return View(teacherVM);
            }

            await _teacherService.CreateAsync(teacherVM);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var teacher = await _teacherService.GetByIdAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }

            var teacherVM = new TeacherUpdateVM
            {
                Id = teacher.Id,
                FullName = teacher.FullName,
                PositionId = teacher.PositionId,
                ExistingImage = teacher.Image
            };

            ViewBag.Positions = new SelectList(await _positionService.GetAllAsync(), "Id", "Name", teacher.PositionId);
            return View(teacherVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, TeacherUpdateVM teacherVM)
        {
            if (id != teacherVM.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Positions = new SelectList(await _positionService.GetAllAsync(), "Id", "Name", teacherVM.PositionId);
                return View(teacherVM);
            }

            await _teacherService.UpdateAsync(teacherVM);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _teacherService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAjax([FromForm] string FullName, IFormFile Image)
        {
            if (string.IsNullOrWhiteSpace(FullName) || Image == null)
            {
                return Json(new { success = false });
            }

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
            string path = Path.Combine(_env.WebRootPath, "images", fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await Image.CopyToAsync(stream);
            }

            var positions = await _positionService.GetAllAsync();
            var firstPosition = positions.FirstOrDefault();

            var teacherVM = new TeacherCreateVM
            {
                FullName = FullName,
                Image = Image,
                PositionId = firstPosition?.Id ?? 1
            };

            await _teacherService.CreateAsync(teacherVM);

            var createdTeacher = (await _teacherService.GetAllAsync()).OrderByDescending(t => t.Id).FirstOrDefault();

            return Json(new { success = true, id = createdTeacher.Id, fullName = createdTeacher.FullName });
        }
    }
}
