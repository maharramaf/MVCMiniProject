using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using MVCMiniProject.Helpers;
using MVCMiniProject.Services.Interfaces;
using MVCMiniProject.ViewModels.Account;

namespace MVCMiniProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectAfterLogin(returnUrl, User.FindFirstValue(ClaimTypes.Role));
            }

            return View(new LoginVM { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _authService.ValidateLoginAsync(model.Email, model.Password);
            if (!result.Ok)
            {
                model.NeedsVerification = result.NeedsVerification;
                ModelState.AddModelError(string.Empty, result.Message);
                return View(model);
            }

            await SignInAsync(result.User!);
            return RedirectAfterLogin(model.ReturnUrl, result.User!.Role);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectAfterLogin(null, User.FindFirstValue(ClaimTypes.Role));
            }

            return View(new RegisterVM());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _authService.RegisterAsync(
                model.FirstName,
                model.LastName,
                model.Email,
                model.Password,
                GetPublicBaseUrl());

            if (!result.Ok)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(model);
            }

            TempData["AuthMessage"] = result.Message;
            return RedirectToAction(nameof(Login), new { registered = 1 });
        }

        [HttpGet("/auth/verify-email")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyEmail(string? token)
        {
            var result = await _authService.ConfirmEmailAsync(token ?? string.Empty);
            ViewBag.Ok = result.Ok;
            ViewBag.Message = result.Message;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [EnableRateLimiting("verify-resend")]
        public async Task<IActionResult> ResendVerification(ResendVerificationVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["AuthError"] = "Enter a valid email address.";
                return RedirectToAction(nameof(Login));
            }

            var result = await _authService.ResendVerificationAsync(model.Email, GetPublicBaseUrl());
            TempData[result.Ok ? "AuthMessage" : "AuthError"] = result.Message;
            return RedirectToAction(nameof(Login));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        [Authorize]
        [HttpGet]
        public IActionResult Profile()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            Response.StatusCode = 403;
            return View();
        }

        private async Task SignInAsync(MVCMiniProject.Models.AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName ?? user.Email),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role ?? "user")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
                });
        }

        private IActionResult RedirectAfterLogin(string? returnUrl, string? role)
        {
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            if (AppRoles.IsStaff(role))
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }

            return RedirectToAction("Index", "Home");
        }

        private string GetPublicBaseUrl()
        {
            return $"{Request.Scheme}://{Request.Host}";
        }
    }
}
