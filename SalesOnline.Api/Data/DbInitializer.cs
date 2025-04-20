using SalesOnline.Domain.Entities;
using SalesOnline.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace SalesOnline.Api.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context, ILogger<Program> logger)
        {
            try
            {
                logger.LogInformation("Iniciando inicialización de la base de datos...");

                // Asegurarse de que la base de datos existe
                await context.Database.EnsureCreatedAsync();
                logger.LogInformation("Base de datos creada o verificada exitosamente.");

                // Verificar si ya hay tours
                if (await context.Tours.AnyAsync())
                {
                    logger.LogInformation("La base de datos ya contiene tours. Saltando la inicialización.");
                    return; // La base de datos ya tiene datos
                }

                logger.LogInformation("Agregando tours de ejemplo...");

                // Agregar tours de ejemplo
                var tours = new Tour[]
                {
                    new Tour
                    {
                        Name = "Aventura en Punta Cana",
                        Description = "Disfruta de las mejores playas del Caribe, actividades acuáticas, y resorts todo incluido. Visita Isla Saona y realiza snorkel en aguas cristalinas.",
                        Destination = "Punta Cana, República Dominicana",
                        StartDate = DateTime.Now.AddDays(30),
                        EndDate = DateTime.Now.AddDays(37),
                        Price = 1299.99m,
                        MaxGroupSize = 20,
                        Duration = 7,
                        ImageUrl = "https://images.unsplash.com/photo-1584132967334-10e028bd69f7",
                        IsAvailable = true,
                        CreatedDate = DateTime.Now
                    },
                    new Tour
                    {
                        Name = "Aventura en Caracas",
                        Description = "Explora la vibrante capital venezolana, visita el Parque Nacional El Ávila, disfruta de la gastronomía local y conoce el casco histórico de la ciudad.",
                        Destination = "Caracas, Venezuela",
                        StartDate = DateTime.Now.AddDays(15),
                        EndDate = DateTime.Now.AddDays(18),
                        Price = 599.99m,
                        MaxGroupSize = 15,
                        Duration = 3,
                        ImageUrl = "https://images.unsplash.com/photo-1583531352515-8884af319dc1",
                        IsAvailable = true,
                        CreatedDate = DateTime.Now
                    }
                };

                await context.Tours.AddRangeAsync(tours);
                await context.SaveChangesAsync();
                logger.LogInformation($"Se agregaron {tours.Length} tours exitosamente.");

                // Agregar un cliente de ejemplo para pruebas
                var customer = new Customer
                {
                    FirstName = "Juan",
                    LastName = "Pérez",
                    Email = "juan.perez@example.com",
                    Phone = "+58 412-1234567",
                    PassportNumber = "AB123456",
                    Nationality = "Venezolano",
                    PreferredLanguage = "Español",
                    CreatedDate = DateTime.Now
                };

                await context.Customers.AddAsync(customer);
                await context.SaveChangesAsync();
                logger.LogInformation("Cliente de ejemplo agregado exitosamente.");

                logger.LogInformation("Inicialización de la base de datos completada con éxito.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error durante la inicialización de la base de datos");
                throw; // Re-lanzar la excepción para que sea manejada por el llamador
            }
        }
    }
}
