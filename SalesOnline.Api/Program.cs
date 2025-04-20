using Microsoft.EntityFrameworkCore;
using SalesOnline.Infrastructure.Context;
using SalesOnline.IOC.Dependencies;
using SalesOnline.Api.Data;
using Microsoft.Extensions.Logging;

namespace SalesOnline.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Add logging
        builder.Logging.AddConsole();
        builder.Logging.AddDebug();

        // Add CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
        });

        // Add dependencies
        builder.Services.AddDependencies(builder.Configuration);

        var app = builder.Build();

        // Initialize Database
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                var logger = services.GetRequiredService<ILogger<Program>>();
                
                logger.LogInformation("Iniciando la aplicación...");
                
                // Asegurarse de que la base de datos está creada y aplicar las migraciones
                logger.LogInformation("Aplicando migraciones...");
                await context.Database.MigrateAsync();
                
                // Inicializar datos
                logger.LogInformation("Inicializando datos...");
                await DbInitializer.Initialize(context, logger);
                logger.LogInformation("Inicialización completada exitosamente.");
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Ocurrió un error durante la inicialización de la aplicación.");
                throw;
            }
        }

        // Configure the HTTP request pipeline.
        app.UseDefaultFiles();
        app.UseStaticFiles();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SalesOnline API V1");
                c.RoutePrefix = "swagger"; // Swagger estará disponible en /swagger
            });
        }

        app.UseCors("AllowAll");
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        await app.RunAsync();
    }
}

