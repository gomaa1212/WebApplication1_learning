using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Globalization;
using WebApplication1.Data;
using WebApplication1.Models.Services.Implementations;
using WebApplication1.Models.Services.Interfaces;
using WebApplication1.Repository.Impelementations;
using WebApplication1.Repository.Interfaces;
using WebApplication1.SharedRepository.Impelementations;
using WebApplication1.SharedRepository.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(x =>
    x.UseSqlServer(builder.Configuration["Constr"]));

// Localization
builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});

builder.Services
    .AddControllersWithViews()
    .AddViewLocalization();

builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IGenderService, GenderService>();
builder.Services.AddScoped<IStudentImagesService, StudentImagesServicem>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IStudentImagesRepository, StudentImagesRepository>();
builder.Services.AddScoped<IGenderRepository,GenderRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.Cookie.IsEssential = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.Name = ".SimpleProject";
    options.Cookie.Path = "/";
    options.IOTimeout = TimeSpan.FromMinutes(5);
});

builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Languages
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("en-US"),
        new CultureInfo("ar-EG")
    };

    options.DefaultRequestCulture = new RequestCulture("ar-EG");

    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    options.RequestCultureProviders.Insert(
        0,
        new CookieRequestCultureProvider()
    );
});

var app = builder.Build();

// Localization MUST be before routing
var localizationOptions =
    app.Services
        .GetRequiredService<IOptions<RequestLocalizationOptions>>()
        .Value;

app.UseRequestLocalization(localizationOptions);

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();