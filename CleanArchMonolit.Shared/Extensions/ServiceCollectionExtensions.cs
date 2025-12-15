using CleanArchMonolit.Shared.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace CleanArchMonolit.Shared.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ApplyCommonSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            return services;
        }
    }
}
