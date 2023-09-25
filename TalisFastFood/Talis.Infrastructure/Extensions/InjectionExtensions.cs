using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Talis.Infrastructure.Persistences.Data;
using Talis.Infrastructure.Persistences.Interfaces;
using Talis.Infrastructure.Persistences.Repositories;

namespace Talis.Infrastructure.Extensions
{
    public static class InjectionExtensions
    {
        public static IServiceCollection AddInjectionInfrastucture(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ITalisContext>(provider =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                return new TalisContext(connectionString);
            });

            services.AddTransient<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
