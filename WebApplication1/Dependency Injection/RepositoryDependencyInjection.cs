using WebApplication1.Repository.Impelementations;
using WebApplication1.Repository.Interfaces;
using WebApplication1.SharedRepository.Impelementations;
using WebApplication1.SharedRepository.Interfaces;

namespace WebApplication1.Dependency_Injection
{
    public static class RepositoryDependencyInjection
    {
        public static IServiceCollection AddRepositoryDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IStudentImagesRepository, StudentImagesRepository>();
            services.AddScoped<IGenderRepository, GenderRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            return services;
        }
    }
}
