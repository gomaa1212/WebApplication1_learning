using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WebApplication1.Data;
using WebApplication1.Models.Services.Implementations;
using WebApplication1.Models.Services.Interfaces;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(builder.Configuration["Constr"]));

            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IStudentService,StudentService>();
            builder.Services.AddScoped<IFileService,FileService>();
            builder.Services.AddScoped<IGenderService,GenderService>();
            builder.Services.AddScoped<IStudentImagesService,StudentImagesServicem>();

            builder.Services.AddAutoMapper(typeof(Program).Assembly);
            var app = builder.Build();


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();


            app.MapControllerRoute(
                name: "default",
                pattern: "{Controller=Home}/{Action=Index}"
                );


            app.Run();
        }
    }
}
