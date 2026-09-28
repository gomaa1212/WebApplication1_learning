using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Globalization;
using WebApplication1.Data;
using WebApplication1.Dependency_Injection;
using WebApplication1.Models.Services.Implementations;
using WebApplication1.Models.Services.Interfaces;
using WebApplication1.Repository.Impelementations;
using WebApplication1.Repository.Interfaces;
using WebApplication1.SharedRepository.Impelementations;
using WebApplication1.SharedRepository.Interfaces;

var builder = WebApplication.CreateBuilder(args);


// Dependency Injection
builder.Services.AddGeneralRegisterDependencyInjection(builder.Configuration);
builder.Services.AddLocalizationDependencyInjection();
builder.Services.AddServiceDependencyInjection();
builder.Services.AddRepositoryDependencyInjection();


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
app.AddAppBuilderDependencyInjection(app.Services);

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();