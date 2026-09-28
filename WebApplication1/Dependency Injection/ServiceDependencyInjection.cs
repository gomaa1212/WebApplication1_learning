using Microsoft.CodeAnalysis.CSharp.Syntax;
using WebApplication1.Models.Services.Implementations;
using WebApplication1.Models.Services.Interfaces;

namespace WebApplication1.Dependency_Injection
{
    public static class ServiceDependencyInjection
    {
        public static IServiceCollection AddServiceDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IGenderService, GenderService>();
            services.AddScoped<IStudentImagesService, StudentImagesServicem>();
            return services;
        }
    }
}
