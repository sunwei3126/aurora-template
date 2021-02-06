using System.Threading.Tasks;

namespace Aurora.Services.Tenants.Demos
{
    public class DemoAppService : AuroraTenantAppService, IDemoAppService
    {
        public Task TestAsync()
        {
            return Task.CompletedTask;
        }
    }
}