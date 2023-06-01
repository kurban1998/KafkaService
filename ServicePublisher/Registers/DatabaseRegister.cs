using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ServicePublisher.Registars
{
    internal static class DatabaseRegister
    {
        public static void AddDbPsql(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("ConnectionString").Value;

            ArgumentNullException.ThrowIfNull(connectionString, nameof(connectionString));

            services.AddDbContext<MyDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });
        }
    }
}
