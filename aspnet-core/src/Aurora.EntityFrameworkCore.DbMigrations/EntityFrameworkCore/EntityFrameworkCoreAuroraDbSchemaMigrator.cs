using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Aurora.Data;
using Volo.Abp.DependencyInjection;

namespace Aurora.EntityFrameworkCore
{
    public class EntityFrameworkCoreAuroraDbSchemaMigrator
        : IAuroraDbSchemaMigrator, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public EntityFrameworkCoreAuroraDbSchemaMigrator(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task MigrateAsync()
        {
            /* We intentionally resolving the AuroraMigrationsDbContext
             * from IServiceProvider (instead of directly injecting it)
             * to properly get the connection string of the current tenant in the
             * current scope.
             */

            await _serviceProvider
                .GetRequiredService<AuroraMigrationsDbContext>()
                .Database
                .MigrateAsync();
        }
    }
}