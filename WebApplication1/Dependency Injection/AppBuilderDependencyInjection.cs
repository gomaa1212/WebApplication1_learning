using Microsoft.Extensions.Options;

namespace WebApplication1.Dependency_Injection
{
    public static class AppBuilderDependencyInjection
    {
        public static IApplicationBuilder AddAppBuilderDependencyInjection(this IApplicationBuilder app,IServiceProvider service)
        {
            // Localization MUST be before routing
            var localizationOptions =
                service
                    .GetRequiredService<IOptions<RequestLocalizationOptions>>()
                    .Value;

            app.UseRequestLocalization(localizationOptions);
            return app;
        }
    }
}
