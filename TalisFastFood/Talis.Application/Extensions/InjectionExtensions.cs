using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Talis.Application.Interfaces;
using Talis.Application.Mappers;
using Talis.Application.Services;

namespace Talis.Application.Extensions
{
    public static class InjectionExtensions
    {
        public static IServiceCollection AddInjectionApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration);

            services.AddAutoMapper(typeof(CategoryMappingsProfile));

            services.AddScoped<IProductApplication, ProductApplication>();

            return services;
        }
    }
}
