namespace WebApplication1.Dependency_Injection
{
    public static class LocalizationDependencyInjection
    {
        public static IServiceCollection AddLocalizationDependencyInjection(this IServiceCollection services)
        {
            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });
            return services;
        }
    }
}
