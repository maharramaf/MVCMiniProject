using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using MVCMiniProject.Data;
using MVCMiniProject.Helpers;
using MVCMiniProject.Models;
using MVCMiniProject.Options;
using MVCMiniProject.Services;
using MVCMiniProject.Services.Interfaces;
using System.Threading.RateLimiting;

DotEnvLoader.Load(Path.Combine(Directory.GetCurrentDirectory(), ".env"));
DotEnvLoader.Load(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", ".env"));

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddInMemoryCollection(DotEnvLoader.ToSmtpConfiguration());

builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();
builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection(SmtpOptions.SectionName));
builder.Services.PostConfigure<SmtpOptions>(options =>
{
    if (int.TryParse(builder.Configuration["Smtp:Port"], out var port))
    {
        options.Port = port;
    }

    if (bool.TryParse(builder.Configuration["Smtp:EnableSsl"], out var ssl))
    {
        options.EnableSsl = ssl;
    }
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.Name = "elearn.auth";
    });

builder.Services.AddAuthorization();
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddFixedWindowLimiter("verify-resend", limiter =>
    {
        limiter.PermitLimit = 3;
        limiter.Window = TimeSpan.FromMinutes(10);
        limiter.QueueLimit = 0;
    });
});

builder.Services.AddScoped<IIconService, IconService>();
builder.Services.AddScoped<ISliderService, SliderService>();
builder.Services.AddScoped<ISettingService, SettingService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<INewsService, NewsService>();
builder.Services.AddScoped<IVideoService, VideoService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IVisionAboutService, VisionAboutService>();
builder.Services.AddScoped<IPlatformAboutService, PlatformAboutService>();
builder.Services.AddScoped<ITeacherService, TeacherService>();
builder.Services.AddScoped<IPositionService, PositionService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAdminCrudService, AdminCrudService>();
builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    if (!db.AppUsers.Any(u => u.Role == AppRoles.Admin))
    {
        db.AppUsers.Add(new AppUser
        {
            Username = "admin@local",
            Password = PasswordHelper.Hash("Admin123!"),
            FullName = "Administrator",
            Email = "admin@local",
            Role = AppRoles.Admin,
            IsEmailVerified = true
        });
        db.SaveChanges();
    }

    if (!db.AppUsers.Any(u => u.Role == AppRoles.SuperAdmin))
    {
        db.AppUsers.Add(new AppUser
        {
            Username = "superadmin@local",
            Password = PasswordHelper.Hash("SuperAdmin123!"),
            FullName = "Super Administrator",
            Email = "superadmin@local",
            Role = AppRoles.SuperAdmin,
            IsEmailVerified = true
        });
        db.SaveChanges();
    }
}

app.UseStaticFiles();
app.UseRouting();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
