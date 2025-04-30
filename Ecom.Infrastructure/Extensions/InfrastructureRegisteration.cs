using Ecom.Application.Interfaces.Repositories;
using Ecom.Infrastructure.Data;
using Ecom.Infrastructure.Implementation.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Extensions
{
    public static class InfrastructureRegisteration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,IConfiguration configuration)
        {
            //apply DbContext
            services.AddDbContext<AppDbContext>(option =>
            {
                option.UseSqlServer(configuration.GetConnectionString("EcomDatabase"));
            });

            // add redis cache configuration to connect to redis server
            services.AddSingleton<IConnectionMultiplexer>(options =>
            {
                var config = ConfigurationOptions.Parse(configuration.GetConnectionString("Redis"));
                return ConnectionMultiplexer.Connect(config);
            });

            //apply UnitOfWork 
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            return services;
        }

    }
}
