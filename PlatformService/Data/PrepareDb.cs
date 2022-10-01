using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data;

public static class PrepareDb
{
    public static void PreparePopulation(this WebApplication app, bool isProduction)
    {
        using var serviceScope = app.Services.CreateScope();

        SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProduction);
    }

    private static void SeedData(AppDbContext? context, bool isProduction)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (isProduction)
        {
            Console.WriteLine("--> Applying migrations...");
            try
            {
                context.Database.Migrate();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not run migrations: {e.Message}");
            }
            
        }

        if (!context.Platforms.Any())
        {
            Console.WriteLine("--> Seeding data...");

            context.Platforms.AddRange(
                new Platform { Name = ".NET", Publisher = "Microsoft", Cost = "Free" },
                new Platform { Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free" },
                new Platform { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free"}
            );

            context.SaveChanges();
        }
        else
        {
            Console.WriteLine("--> We already have data");
        }
    }
}