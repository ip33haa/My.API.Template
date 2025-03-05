using EFCore.AutomaticMigrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Template.Infrastructure.Data;
using Template.Infrastructure.SeedData;

namespace Template.Infrastructure.Extensions
{
    public static class StartupDbExtensions
    {
        public static async Task CreateDbIfNotExistsAsync(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("StartupDbExtensions"); // Use a string category name instead

            try
            {
                var isoContext = services.GetRequiredService<TemplateDbContext>();

                logger.LogInformation("Ensuring database is created...");
                await isoContext.Database.EnsureCreatedAsync();

                // Apply automatic migrations
                logger.LogInformation("Applying migrations...");
                await isoContext.MigrateToLatestVersionAsync();

                // Seed database
                logger.LogInformation("Seeding database...");
                await DBInitializerSeedData.InitializeDatabaseAsync(isoContext);

                logger.LogInformation("Database setup completed successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while initializing the database.");
                throw;
            }
        }
    }
}
