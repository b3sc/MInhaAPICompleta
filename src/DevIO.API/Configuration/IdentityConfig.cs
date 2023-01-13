using DevIO.API.Data;
using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace DevIO.API.Configuration
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }
    }
}

