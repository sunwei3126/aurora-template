using System.Threading.Tasks;

namespace Aurora.Services.Hosts.Demos
{
    public class DemoAppService : AuroraHostAppService, IDemoAppService
    {
        public Task TestAsync()
        {
            return Task.CompletedTask;
        }
    }
}