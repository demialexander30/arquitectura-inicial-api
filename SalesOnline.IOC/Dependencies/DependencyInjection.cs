using Microsoft.Extensions.DependencyInjection;
using SalesOnline.Domain.Core;
using SalesOnline.Domain.Interfaces;
using SalesOnline.Infrastructure;

namespace SalesOnline.IOC.Dependencies
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            return services;
        }
    }
}
