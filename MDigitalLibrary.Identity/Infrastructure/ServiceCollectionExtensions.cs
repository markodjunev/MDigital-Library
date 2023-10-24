namespace MDigitalLibrary.Identity.Infrastructure
{
    using Microsoft.AspNetCore.Identity;
    using MDigitalLibrary.Identity.Data;
    using MDigitalLibrary.Identity.Data.Models;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUserStorage(
            this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequiredLength = 6;
            })
                .AddEntityFrameworkStores<IdentityDbContext>();

            return services;
        }
    }
}
