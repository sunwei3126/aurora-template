using System;
using System.Threading.Tasks;
using Aurora.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Aurora.EntityFrameworkCore
{
    public class EntityFrameworkCoreAuroraDbSchemaMigrator : IAuroraDbSchemaMigrator, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public EntityFrameworkCoreAuroraDbSchemaMigrator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task MigrateAsync()
        {
            await _serviceProvider.GetRequiredService<AuroraMigrationsDbContext>().Database.MigrateAsync();
        }
    }
}