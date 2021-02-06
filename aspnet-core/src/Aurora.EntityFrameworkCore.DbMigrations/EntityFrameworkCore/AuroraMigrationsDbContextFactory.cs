using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Aurora.EntityFrameworkCore
{
    public class AuroraMigrationsDbContextFactory : IDesignTimeDbContextFactory<AuroraMigrationsDbContext>
    {
        public AuroraMigrationsDbContext CreateDbContext(string[] args)
        {
            AuroraEfCoreEntityExtensionMappings.Configure();

            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<AuroraMigrationsDbContext>()
                .UseNpgsql(configuration.GetConnectionString("Default"));

            return new AuroraMigrationsDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Aurora.DbMigrator/"))
                .AddJsonFile("appsettings.json", false);

            return builder.Build();
        }
    }
}