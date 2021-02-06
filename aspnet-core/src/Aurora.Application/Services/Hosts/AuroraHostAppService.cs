using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aurora.Services.Hosts
{
    [Authorize("aurora-host-api")]
    [ApiExplorerSettings(GroupName = "host")]
    public abstract class AuroraHostAppService : AuroraAppService
    {
    }
}