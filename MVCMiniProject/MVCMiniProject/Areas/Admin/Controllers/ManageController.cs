using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCMiniProject.Admin;
using MVCMiniProject.Helpers;
using MVCMiniProject.Services.Interfaces;
using MVCMiniProject.ViewModels.Admin;
using System.Threading.Tasks;

namespace MVCMiniProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = AppRoles.Staff)]
    [Route("admin/{entity}")]
    public class ManageController : Controller
    {
        private readonly IAdminCrudService _adminCrudService;

        public ManageController(IAdminCrudService adminCrudService)
        {
            _adminCrudService = adminCrudService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(string entity, string? search, string? sort, string? dir, int page = 1)
        {
            if (AdminCatalog.Find(entity) == null)
            {
                return NotFound();
            }

            var model = await _adminCrudService.GetListAsync(entity, search, sort, dir, page, Request.Query);
            return View(model);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Detail(string entity, int id)
        {
            var model = await _adminCrudService.GetDetailAsync(entity, id);
            if (model == null)
            {
                TempData["Error"] = "Record not found.";
                return RedirectToAction(nameof(Index), new { entity });
            }

            return View(model);
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create(string entity)
        {
            var model = await _adminCrudService.GetFormAsync(entity, null);
            if (model == null)
            {
                return NotFound();
            }

            return View("Form", model);
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string entity, IFormCollection form)
        {
            var result = await _adminCrudService.CreateAsync(entity, form, Request.Form.Files);
            if (result.Ok)
            {
                TempData["Success"] = result.Message;
                return RedirectToAction(nameof(Index), new { entity });
            }

            var model = await RebuildFormAsync(entity, null, form, result.Message);
            return View("Form", model);
        }

        [HttpGet("{id:int}/edit")]
        public async Task<IActionResult> Edit(string entity, int id)
        {
            var model = await _adminCrudService.GetFormAsync(entity, id);
            if (model == null)
            {
                TempData["Error"] = "Record not found.";
                return RedirectToAction(nameof(Index), new { entity });
            }

            return View("Form", model);
        }

        [HttpPost("{id:int}/edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string entity, int id, IFormCollection form)
        {
            var result = await _adminCrudService.UpdateAsync(entity, id, form, Request.Form.Files);
            if (result.Ok)
            {
                TempData["Success"] = result.Message;
                return RedirectToAction(nameof(Index), new { entity });
            }

            var model = await RebuildFormAsync(entity, id, form, result.Message);
            return View("Form", model);
        }

        [HttpPost("{id:int}/delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string entity, int id)
        {
            var result = await _adminCrudService.DeleteAsync(entity, id);
            TempData[result.Ok ? "Success" : "Error"] = result.Message;
            return RedirectToAction(nameof(Index), new { entity });
        }

        private async Task<AdminFormVM> RebuildFormAsync(string entity, int? id, IFormCollection form, string error)
        {
            var model = await _adminCrudService.GetFormAsync(entity, id) ?? new AdminFormVM
            {
                Entity = AdminCatalog.Find(entity)!,
                Id = id,
                IsEdit = id.HasValue
            };

            foreach (var field in model.Entity.Fields.Where(f => f.ShowInForm && f.Kind != AdminFieldKind.Image && f.Kind != AdminFieldKind.Password))
            {
                model.Values[field.Name] = form[field.Name].ToString();
            }

            TempData["Error"] = error;
            return model;
        }
    }
}
