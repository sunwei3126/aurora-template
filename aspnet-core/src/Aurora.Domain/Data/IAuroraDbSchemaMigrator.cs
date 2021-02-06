using System.Threading.Tasks;

namespace Aurora.Data
{
    public interface IAuroraDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}
