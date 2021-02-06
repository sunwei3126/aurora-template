using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Aurora.Services.Hosts.Demos
{
    public interface IDemoAppService : IApplicationService
    {
        Task TestAsync();
    }
}