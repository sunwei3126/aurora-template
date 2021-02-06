using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Aurora.Data
{
    /* This is used if database provider does't define
     * IAuroraDbSchemaMigrator implementation.
     */
    public class NullAuroraDbSchemaMigrator : IAuroraDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}