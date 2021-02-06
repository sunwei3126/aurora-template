using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Aurora.Data
{
    public class NullAuroraDbSchemaMigrator : IAuroraDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}