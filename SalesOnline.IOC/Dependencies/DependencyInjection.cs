using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SalesOnline.Domain.Core;
using SalesOnline.Domain.Interfaces;
using SalesOnline.Domain.Repositories;
using SalesOnline.Domain.Context;
using SalesOnline.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace SalesOnline.IOC.Dependencies
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // Database Context
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Repositories
            services.AddScoped<ITourRepository, TourRepository>();
            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

            return services;
        }
    }
}
