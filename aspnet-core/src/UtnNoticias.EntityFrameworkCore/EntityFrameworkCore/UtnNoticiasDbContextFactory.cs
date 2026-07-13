using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace UtnNoticias.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class UtnNoticiasDbContextFactory : IDesignTimeDbContextFactory<UtnNoticiasDbContext>
{
    public UtnNoticiasDbContext CreateDbContext(string[] args)
    {
        UtnNoticiasEfCoreEntityExtensionMappings.Configure();

        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<UtnNoticiasDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));

        return new UtnNoticiasDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(GetDbMigratorProjectPath())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }

    private static string GetDbMigratorProjectPath()
    {
        var currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

        while (currentDirectory != null)
        {
            var srcCandidate = Path.Combine(currentDirectory.FullName, "src", "UtnNoticias.DbMigrator");
            if (File.Exists(Path.Combine(srcCandidate, "appsettings.json")))
            {
                return srcCandidate;
            }

            var siblingCandidate = Path.Combine(currentDirectory.FullName, "UtnNoticias.DbMigrator");
            if (File.Exists(Path.Combine(siblingCandidate, "appsettings.json")))
            {
                return siblingCandidate;
            }

            currentDirectory = currentDirectory.Parent;
        }

        throw new DirectoryNotFoundException("Could not find UtnNoticias.DbMigrator/appsettings.json.");
    }
}
