using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using WebApplication1.Data;

namespace WebApplication1.Dependency_Injection
{
    public static class GeneralRegisterDependencyInjection
    {
        public static IServiceCollection AddGeneralRegisterDependencyInjection(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(x =>x.UseSqlServer(configuration["Constr"]));

            services
                   .AddControllersWithViews()
                   .AddViewLocalization();

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.Cookie.IsEssential = true;
                options.Cookie.HttpOnly = true;
                options.Cookie.Name = ".SimpleProject";
                options.Cookie.Path = "/";
                options.IOTimeout = TimeSpan.FromMinutes(5);
            });

            services.AddAutoMapper(typeof(Program).Assembly);

          

            return services;
        }
    }
}
