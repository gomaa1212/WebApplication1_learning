using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using WebApplication1.Data;
using WebApplication1.Models.Identity;

namespace WebApplication1.Dependency_Injection
{
    public static class IdentityDependencyInjection
    {
        public static IServiceCollection AddIdentityDependencyInjection(this IServiceCollection services)
        {

            services.AddIdentity<User, IdentityRole>(opt=>
            {
                //password setting
                opt.Password.RequireLowercase = true;
                opt.Password.RequireDigit = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequiredLength = 6;
                opt.Password.RequireNonAlphanumeric = false;

                //Lock out setting
                opt.Lockout.MaxFailedAccessAttempts = 6;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                

                //user setting
                opt.User.RequireUniqueEmail = true;
               

                //signin
                opt.SignIn.RequireConfirmedEmail = true;
                
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            services.AddAuthorization(options =>
            {
            options.AddPolicy("CreateStudent", policy => policy.RequireAssertion(
                context => context.User.IsInRole("Admin") && context.User.HasClaim("Add Student", "True")));

             options.AddPolicy("CreateRole", policy => policy.RequireClaim("Create role"));


            });
            
            return services;
        }
    }
}
