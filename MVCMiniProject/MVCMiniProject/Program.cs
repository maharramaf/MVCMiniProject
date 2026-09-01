using Microsoft.EntityFrameworkCore;

using MVCMiniProject.Data;
using MVCMiniProject.Services;
using MVCMiniProject.Services.Interfaces;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IIconService, IconService>();
builder.Services.AddScoped<ISliderService , SliderService>();
builder.Services.AddScoped<ISettingService, SettingService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<INewsService, NewsService>();
builder.Services.AddScoped<IVideoService, VideoService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IVisionAboutService, VisionAboutService>();
builder.Services.AddScoped<IPlatformAboutService, PlatformAboutService>();
builder.Services.AddScoped<ITeacherService, TeacherService>();


var app = builder.Build();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();