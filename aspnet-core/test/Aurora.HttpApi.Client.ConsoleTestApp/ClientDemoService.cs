using System;
using System.Threading.Tasks;
using Aurora.Services.Hosts.Demos;
using Volo.Abp.DependencyInjection;

namespace Aurora.HttpApi.Client.ConsoleTestApp
{
    public class ClientDemoService : ITransientDependency
    {
        private readonly IDemoAppService _demoAppService;

        public ClientDemoService(IDemoAppService demoAppService)
        {
            _demoAppService = demoAppService;
        }

        public async Task RunAsync()
        {
            await _demoAppService.TestAsync();
            Console.WriteLine("Call Host DemoAppService TestAsync Success");
        }
    }
}