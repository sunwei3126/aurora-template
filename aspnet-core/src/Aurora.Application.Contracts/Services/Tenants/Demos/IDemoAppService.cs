using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Aurora.Services.Tenants.Demos
{
    public interface IDemoAppService : IApplicationService
    {
        Task TestAsync();
    }
}