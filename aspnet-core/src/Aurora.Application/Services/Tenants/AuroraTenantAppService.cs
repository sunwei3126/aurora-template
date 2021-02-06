using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aurora.Services.Tenants
{
    [Authorize("aurora-api")]
    [ApiExplorerSettings(GroupName = "aurora")]
    public abstract class AuroraTenantAppService : AuroraAppService
    {
    }
}