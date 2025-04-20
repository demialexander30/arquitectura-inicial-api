using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SalesOnline.Domain.Interfaces;
using SalesOnline.Infrastructure.Context;
using SalesOnline.Infrastructure.Repositories;
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
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            return services;
        }
    }
}
